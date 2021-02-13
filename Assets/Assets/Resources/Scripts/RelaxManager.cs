using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using DoozyUI;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public enum RelaxBubbleColor { Normal, Pink, Green, Yellow, Blue };
public enum RelaxBubbleSize { SuperSmall, Small, Normal, Big };

public class RelaxManager : MonoBehaviour {
    Dictionary<RelaxBubbleSize, float> sizeToScale = new Dictionary<RelaxBubbleSize, float> {
        { RelaxBubbleSize.Big, 5.0f },
        { RelaxBubbleSize.Normal, 5.8f },
        { RelaxBubbleSize.Small, 7.8f },
        { RelaxBubbleSize.SuperSmall, 8.8f }
      };
    Dictionary<RelaxBubbleSize, int> sizeToHeigh = new Dictionary<RelaxBubbleSize, int> {
        { RelaxBubbleSize.Big, 48 },
        { RelaxBubbleSize.Normal, 50 },
        { RelaxBubbleSize.Small, 52 },
        { RelaxBubbleSize.SuperSmall, 64 }
      };

    private static RelaxManager instance;
    private static object _lock = new object();

    public AudioSource audioSource;

    public GameObject player;

    // textures ans sounds for bursted bubbles
    public AudioClip[] mainTrack;
    public Sprite[] goodBubblesSprites;
    public AudioClip[] goodBubbleSounds;

    // UI elements
    //public GameObject startFinishPanel;
    //public GameObject startPanel;
    //public GameObject finishPanel;
    //public GameObject topPanel;
    //public GameObject counterObject;
    //public GameObject buttonsPanel;
    //public GameObject pauseButton;
    //public GameObject nextButton;
    //public Text finishText;
    //public GameObject photoButton;


    //public GameObject menuBackground;
    public GameObject menuPanel;
    public GameObject configButton;
    public GameObject tapHereText;
    public RelaxMenuController menuController;
    public GameObject backButton;
    //public GameObject relaxButton;
    //public GameObject settingsHeader;
    //public GameObject backButtonIngame;

    //public Text scoresText;
    //public Text goodBubblesText;


    public RelaxGridManager _gridManager;
    public bool starting = false;
    public bool started = false;
    public bool finished = false;
    public float startTime = 0;


    public Sprite normalBubbleSprite;
    public Sprite blueBubbleSprite;
    public Sprite pinkBubbleSprite;
    public Sprite yellowBubbleSprite;
    public Sprite greenBubbleSprite;

    public Sprite[] burstNormalBubble;
    public Sprite[] burstBlueBubble;
    public Sprite[] burstPinkBubble;
    public Sprite[] burstYellowBubble;
    public Sprite[] burstGreenBubble;


    public HashSet<RelaxBubbleColor> selectedBubbleColors;
    public GameObject counterText;
    public GameObject summaryCounterText;
    public GameObject helpButton;

    private PauseScript _pauseScript;

    private static bool applicationIsQuitting = false;
    private int burstedCounter = 0;
    public RelaxBubbleSize currentBubbleSize = RelaxBubbleSize.Normal;

    //public InterstitialAd interstitial;
    private const string _finishAds = "ca-app-pub-1665385743848421/6400348799";

    private bool isCameraSizeChanging;
    private bool isCameraSizeReducing;
    public static RelaxManager Instance
    {
        get
        {
            if (applicationIsQuitting) {
                Debug.LogWarning("[Singleton] Instance '"+ typeof(RelaxManager) +
                    "' already destroyed on application quit." +
                    " Won't create again - returning null.");
                return null;
            }

            lock(_lock)
            {
                if (instance == null)
                {
                    instance = (RelaxManager) FindObjectOfType(typeof(RelaxManager));

                    if ( FindObjectsOfType(typeof(RelaxManager)).Length > 1 )
                    {
                        Debug.LogError("[Singleton] Something went really wrong " +
                            " - there should never be more than 1 singleton!" +
                            " Reopening the scene might fix it.");
                        return instance;
                    }

                    if (instance == null)
                    {
                        GameObject singleton = new GameObject();
                        instance = singleton.AddComponent<RelaxManager>();
                        singleton.name = "(singleton) "+ typeof(RelaxManager).ToString();

                        DontDestroyOnLoad(singleton);

                        Debug.Log("[Singleton] An instance of " + typeof(RelaxManager) + 
                            " is needed in the scene, so '" + singleton +
                            "' was created with DontDestroyOnLoad.");
                    } else {
                        Debug.Log("[Singleton] Using instance already created: " +
                            instance.gameObject.name);
                    }
                }
                return instance;
            }
        }
    }

    public void OnDestroy () {
        applicationIsQuitting = true;
    }

    void Awake() {
        applicationIsQuitting = false;
    }

    IEnumerator InitGame (){
        selectedBubbleColors = new HashSet<RelaxBubbleColor>();
        selectedBubbleColors.Add(RelaxBubbleColor.Normal);
        summaryCounterText.GetComponent<Text>().text = "/" + (_gridManager.gridWidthInHexes * _gridManager.gridHeightInHexes).ToString();

        audioSource.clip = mainTrack[UnityEngine.Random.Range(0, mainTrack.Length)];
        if (PlayerPrefs.HasKey("isMusicOff") && bool.Parse(PlayerPrefs.GetString("isMusicOff")))
        {
            audioSource.Pause();
        }
        else
        {
            audioSource.Play();
        }
        _gridManager = this.GetComponent<RelaxGridManager>();
        _gridManager.SetUpGrid(sizeToHeigh[currentBubbleSize]);
        player.transform.position = new Vector3(_gridManager.cameraCenter.x, _gridManager.cameraCenter.y, -10);

        //_pauseScript = pauseButton.GetComponent<PauseScript>();

        yield return new WaitForSeconds(0.5f);
        menuPanel.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        tapHereText.GetComponent<UIElement>().Show(false);

        if (UnityEngine.Random.Range(1, 7) == 1) RequestInterstitialAds ();

    }

    public void Start()
    {
        Time.timeScale = 1.0f;
        configButton.GetComponent<UIElement>().Hide(true);
        StartCoroutine("InitGame");
        started = true;
    }



    void Update() {

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (helpButton.GetComponent<OpenHelp>().IsOpened())
            {
                helpButton.GetComponent<OpenHelp>().OnClick();
            }
            else if (menuController.IsOpened())
            {
                backButton.GetComponent<UIButton>().OnClick.Invoke();
            }
            else
            {
                Application.Quit();
            }
        }

        if (isCameraSizeChanging)
        {
            if (isCameraSizeReducing)
            {
                player.GetComponent<Camera>().orthographicSize = player.GetComponent<Camera>().orthographicSize - 1.7f* Time.deltaTime;
                if (player.GetComponent<Camera>().orthographicSize < sizeToScale[currentBubbleSize])
                {
                    player.GetComponent<Camera>().orthographicSize = sizeToScale[currentBubbleSize];
                    isCameraSizeChanging = false;
                }
                    
            }
            else
            {
                player.GetComponent<Camera>().orthographicSize = player.GetComponent<Camera>().orthographicSize + 1.7f * Time.deltaTime;
                if (player.GetComponent<Camera>().orthographicSize > sizeToScale[currentBubbleSize])
                {
                    player.GetComponent<Camera>().orthographicSize = sizeToScale[currentBubbleSize];
                    isCameraSizeChanging = false;
                }
            }
        }
            
    }

    public void OnSizeChanged(RelaxBubbleSize size)
    {
        currentBubbleSize = size;
        isCameraSizeChanging = true;
        isCameraSizeReducing = (player.GetComponent<Camera>().orthographicSize > sizeToScale[currentBubbleSize]) ? true : false;
        UpdateGrid();
        RepaintBubbles();
    }

    void UpdateGrid()
    {
        _gridManager = this.GetComponent<RelaxGridManager>();
        _gridManager.ChangeGridShape(sizeToHeigh[currentBubbleSize]);
        player.transform.position = new Vector3(_gridManager.cameraCenter.x, _gridManager.cameraCenter.y, -10);
        burstedCounter = 0;
        counterText.GetComponent<Text>().text = burstedCounter.ToString();
        summaryCounterText.GetComponent<Text>().text = "/" + (_gridManager.gridWidthInHexes * _gridManager.gridHeightInHexes).ToString();
    }

    public void RequestInterstitialAds()
    {
        //interstitial = new InterstitialAd(_finishAds);
        //AdRequest request = new AdRequest.Builder().AddTestDevice(AdRequest.TestDeviceSimulator).AddTestDevice("7674860455C6D237").AddTestDevice("639F9DCDBC7EFF08").Build();
        //interstitial.LoadAd(request);

    }


    public void BurstRelaxBubble(GameObject obj){
        //set on of the "bursted" sprites to good bubble
        if (starting) {
            return;
        }

        switch (obj.GetComponent<RelaxBubbleClick>().color)
        {
            case RelaxBubbleColor.Normal:
                obj.GetComponent<SpriteRenderer>().sprite = burstNormalBubble[UnityEngine.Random.Range(0, burstNormalBubble.Length)];
                break;
            case RelaxBubbleColor.Blue:
                obj.GetComponent<SpriteRenderer>().sprite = burstBlueBubble[UnityEngine.Random.Range(0, burstBlueBubble.Length)];
                break;
            case RelaxBubbleColor.Green:
                obj.GetComponent<SpriteRenderer>().sprite = burstGreenBubble[UnityEngine.Random.Range(0, burstGreenBubble.Length)];
                break;
            case RelaxBubbleColor.Pink:
                obj.GetComponent<SpriteRenderer>().sprite = burstPinkBubble[UnityEngine.Random.Range(0, burstPinkBubble.Length)];
                break;
            case RelaxBubbleColor.Yellow:
                obj.GetComponent<SpriteRenderer>().sprite = burstYellowBubble[UnityEngine.Random.Range(0, burstYellowBubble.Length)];
                break;
        }
        
        if (!bool.Parse(PlayerPrefs.GetString("isSoundOff", "false"))) {
            obj.GetComponent<AudioSource> ().clip = goodBubbleSounds [UnityEngine.Random.Range (0, goodBubbleSounds.Length)];
            obj.GetComponent<AudioSource> ().Play ();
        }

        obj.GetComponent<EventTrigger>().enabled = false;

        burstedCounter += 1;
        //counterText.GetComponent<UIElement>().Hide(true);
        counterText.GetComponent<Text>().text = burstedCounter.ToString();
        //counterText.GetComponent<UIElement>().Show(false);
    }

    public void OnColorAdd(RelaxBubbleColor color)
    {
        selectedBubbleColors.Add(color);
        RepaintBubbles();
    }

    public void OnColorRemove(RelaxBubbleColor color)
    {
        selectedBubbleColors.Remove(color);
        RepaintBubbles();
    }

    void RepaintBubbles()
    {
        burstedCounter = 0;
        counterText.GetComponent<Text>().text = burstedCounter.ToString();
        summaryCounterText.GetComponent<Text>().text = "/" + (_gridManager.gridWidthInHexes * _gridManager.gridHeightInHexes).ToString();

        RelaxBubbleColor[] colors = new RelaxBubbleColor[selectedBubbleColors.Count];
        selectedBubbleColors.CopyTo(colors);

        int colorToUse = 0;

        for(int i = 0; i < _gridManager.bubblesArray.Count; i++)
        {
            if (i % _gridManager.gridWidthInHexes == 0)
            {
                colorToUse = 0;
            }

            GameObject go = (GameObject)_gridManager.bubblesArray[i];
            go.GetComponent<EventTrigger>().enabled = true;
            switch (colors[colorToUse])
            {
                case RelaxBubbleColor.Normal:
                    go.GetComponent<SpriteRenderer>().sprite = normalBubbleSprite;
                    go.GetComponent<RelaxBubbleClick>().color = RelaxBubbleColor.Normal;
                    break;
                case RelaxBubbleColor.Blue:
                    go.GetComponent<SpriteRenderer>().sprite = blueBubbleSprite;
                    go.GetComponent<RelaxBubbleClick>().color = RelaxBubbleColor.Blue;
                    break;
                case RelaxBubbleColor.Green:
                    go.GetComponent<SpriteRenderer>().sprite = greenBubbleSprite;
                    go.GetComponent<RelaxBubbleClick>().color = RelaxBubbleColor.Green;
                    break;
                case RelaxBubbleColor.Pink:
                    go.GetComponent<SpriteRenderer>().sprite = pinkBubbleSprite;
                    go.GetComponent<RelaxBubbleClick>().color = RelaxBubbleColor.Pink;
                    break;
                case RelaxBubbleColor.Yellow:
                    go.GetComponent<SpriteRenderer>().sprite = yellowBubbleSprite;
                    go.GetComponent<RelaxBubbleClick>().color = RelaxBubbleColor.Yellow;
                    break;
            }

            colorToUse = (colorToUse < colors.Length - 1) ? colorToUse + 1 : 0;

        }
    }

}
