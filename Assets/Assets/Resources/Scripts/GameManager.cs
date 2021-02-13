using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.SocialPlatforms;
using DoozyUI;
using UnityEngine.Advertisements;
using UnityEngine.Android;


public class GameManager : MonoBehaviour {
    enum CarmaStates{Good, Neutral, Bad};

    private static GameManager instance;
    private static object _lock = new object();

    public AudioSource audioSource;

    // vector to move the camera
    public Vector3 moveUnitsPerSecond = new Vector3(0, -1, 0);
    public GameObject player;

    public Text scoreText;
    public Text timeText;
    public GameObject scoreAdderImage;
    public GameObject starObject;
    public Image carmaLeftImage;
    public Image carmaRightImage;

    // textures ans sounds for bursted bubbles
    public AudioClip[] mainTrack;
    public Sprite[] goodBubblesSprites;
    public Sprite[] badBubblesSprites;
    public Sprite[] carmaBubblesSprites;
    public Sprite[] fastBubblesSprites;
    public Sprite[] freeBubblesSprites;
    public Sprite[] scoreBubblesSprites;
    public Sprite[] slowBubblesSprites;

    public Sprite[] ghostBubblesSprites;
    public Sprite[] poisonBubblesSprites;
    public Sprite[] fireBubblesSprites;

    public AudioClip[] badBubbleSounds;
    public AudioClip[] goodBubbleSounds;
    public AudioClip flySound;

    // UI elements
    public GameObject startFinishPanel;
    public GameObject startPanel;
    public GameObject finishPanel;
    public GameObject topPanel;
    public GameObject counterObject;
    public GameObject buttonsPanel;
    public GameObject highScoreImage;
    public GameObject pauseButton;
    public GameObject nextButton;
    public Text finishText;
    public GameObject photoButton;
    public GameObject badCarmaAlert;
    public GameObject bonusImage;
    public Text bonusText;
    public Image bonusTimerImage;

    public GameObject menuBackground;
    public GameObject logoImage;
    public GameObject menuPanel;
    public GameObject startButton;
    public GameObject menuBackgroundLight;
    public GameObject relaxButton;
    public GameObject settingsHeader;
    public GameObject backButtonIngame;
    public MenuController menuController;

    public Text scoresText;
    public Text goodBubblesText;
    public Text badBubblesText;
    public Text finishTimeText;
    public OpenHelp helpButton;
    public ConfigController configController;

    public GridManager _gridManager;
    public bool starting = false;
    public bool started = false;
    public bool finished = false;
    public float startTime = 0;
    public bool isViewingHelp = false;

    private int _score = 0;
    private float _carma = 0;
    private int _goodBubblesBurst = 0;
    private int _badBubblesBurst = 0;
    private int _vangaCookiesBursted;
    private int _fliesCount = 0;
    private float _nextTime = 0;
    private int minutes;
    private int seconds;

    // переменные для ачивок
    private float _timeLastFail;
    private float _timeLastBursted;
    private float _timeLastHelpSpawn;
    private short _burstedInMultiTouch;
    private bool _isBadCarma = false;
    private bool isRecord = false;

    private PauseScript _pauseScript;

    private static bool applicationIsQuitting = false;

    public Bonus bonus = new Bonus(BonusType.Score, 0f, 0);
    public Bonus freeBonus = new Bonus (BonusType.Free, 0f, 0);
    public Bonus visualBonus = new Bonus (BonusType.Ghost, 0f, 0);


    public bool IsPaused
    {
        get
        {
            return _pauseScript.IsPaused;
        }
        set
        {
            _pauseScript.IsPaused = value;
        }
    }

    public static GameManager Instance
    {
        get
        {
            if (applicationIsQuitting) {
                Debug.LogWarning("[Singleton] Instance '"+ typeof(GameManager) +
                    "' already destroyed on application quit." +
                    " Won't create again - returning null.");
                return null;
            }

            lock(_lock)
            {
                if (instance == null)
                {
                    instance = (GameManager) FindObjectOfType(typeof(GameManager));

                    if ( FindObjectsOfType(typeof(GameManager)).Length > 1 )
                    {
                        Debug.LogError("[Singleton] Something went really wrong " +
                            " - there should never be more than 1 singleton!" +
                            " Reopening the scene might fix it.");
                        return instance;
                    }

                    if (instance == null)
                    {
                        GameObject singleton = new GameObject();
                        instance = singleton.AddComponent<GameManager>();
                        singleton.name = "(singleton) "+ typeof(GameManager).ToString();

                        DontDestroyOnLoad(singleton);

                        Debug.Log("[Singleton] An instance of " + typeof(GameManager) + 
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
        Advertisement.Initialize("1757789");
    }

    IEnumerator CheckFrameAchivements(){
        Social.LoadAchievements(achievements => {
            if (achievements != null && achievements.Length > 0)
            {
                foreach (IAchievement achievement in achievements)
                {
                    switch (achievement.id){
                    case GooglePlayActions.longLiveAchiev:
                        PlayerPrefs.SetInt("KingFrame", 1);
                            Debug.Log("KingFrame added");
                            break;        
                    case GooglePlayActions.pioneerAchiev:
                        PlayerPrefs.SetInt("PinkFrame", 1);
                            Debug.Log("PinkFrame added");
                            break;
                    case GooglePlayActions.beginnerAdsAchiev:
                        PlayerPrefs.SetInt("MetallFrame", 1);
                            Debug.Log("MetallFrame added");
                            break;
                    case GooglePlayActions.intermAdsAchiev:
                            PlayerPrefs.SetInt("RainbowFrame", 1);
                            Debug.Log("RainbowFrame added");
                            break;
                    case GooglePlayActions.advancedAdsAchiev:
                            PlayerPrefs.SetInt("GotFrame", 1);
                            Debug.Log("GotFrame added");
                            break;
                    case GooglePlayActions.superAdvancedAdsAchiev:
                        PlayerPrefs.SetInt("CarpetFrame", 1);
                        Debug.Log("CarpetFrame added");
                        break;
                    case GooglePlayActions.mindBlowingAdsAchiev:
                            PlayerPrefs.SetInt("GoldenFrame", 1);
                            Debug.Log("GoldenFrame added");
                            break;
                        case GooglePlayActions.superhumanAchiev:
                            PlayerPrefs.SetInt("HeroFrame", 1);
                            Debug.Log("HeroFrame added");
                            break;
                  case GooglePlayActions.davidBlaineAchiev:
                            PlayerPrefs.SetInt("DragonFrame", 1);
                            Debug.Log("DragonFrame added");
                            break;
                    }
                }
            }
            else
                Debug.Log("No achievements returned");
        });
        yield return null;
    }


    void InitGame (){
        audioSource.clip = mainTrack[UnityEngine.Random.Range(0, mainTrack.Length)];
        if (bool.Parse(PlayerPrefs.GetString("isMusicOff", "false")))
        {
            audioSource.Pause();
        }
        else
        {
            audioSource.Play();
        }
        _gridManager = this.GetComponent<GridManager>();
        _gridManager.SetUpGrid();
        _pauseScript = pauseButton.GetComponent<PauseScript>();
    }

    public void Start()
    {
        InitGame();
        StartCoroutine("AnimateStart");
    }

    IEnumerator AnimateStart()
    {
        Time.timeScale = 1.0f;
        yield return new WaitForSeconds(0.5f);
        menuBackground.SetActive(true);
        menuBackgroundLight.SetActive(true);
        logoImage.SetActive(true);
        startButton.SetActive(true);
        yield return new WaitForSeconds(0.8f);
        menuPanel.SetActive(true);
    }

    IEnumerator AnimatePlay()
    {
        //startFinishPanel.SetActive(true);
        menuPanel.GetComponent<UIElement>().Hide(false);
        menuBackground.GetComponent<UIElement>().Hide(false);
        menuBackgroundLight.GetComponent<UIElement>().Hide(false);
        logoImage.GetComponent<UIElement>().Hide(false);
        startButton.GetComponent<UIElement>().Hide(false);
        //startPanel.SetActive(false);
        yield return new WaitForSeconds(1f);
        topPanel.SetActive(true);
        relaxButton.SetActive(false);
        if (!PlayerPrefs.HasKey("HelpSeen"))
        {
            yield return new WaitForSeconds(2f);
            helpButton.OnClick();
            isViewingHelp = true;
        }
        else
        {
            StartCoroutine(StartGame());
        }
    }

    public void Play()
    {
        // show help 
        StartCoroutine("AnimatePlay");
    }

    public IEnumerator StartGame(){
        StartCoroutine("CheckFrameAchivements");
        started = true;
        starting = true;
        Time.timeScale = 1.0f;
        pauseButton.GetComponent<Button>().interactable = false;
        counterObject.SetActive (true);
        startTime = 0;
        Text counterText = counterObject.GetComponent<Text> ();
        UIElement counterElement = counterObject.GetComponent<UIElement>();
        for (int i = 3; i > 0; i--) {
            counterText.text = i.ToString ();
            counterObject.GetComponent<UIElement>().Show(false);
            yield return new WaitForSeconds(1.5f);
            counterObject.GetComponent<UIElement>().Hide(false);
        }
        counterElement.Show(false);
        bonusImage.GetComponent<Animator>().enabled = true;
        //startFinishPanel.SetActive (false);
        counterObject.SetActive (false);
        starting = false;
        _nextTime = Time.time + 1;
        pauseButton.GetComponent<Button>().interactable = true;
        _timeLastHelpSpawn = Time.time;
        _timeLastBursted = 0;
        //unblockAllFrames(); // TODO remove after testing
    }

    void UnblockAllFrames()
    {
        PlayerPrefs.SetInt("PinkFrame", 1);
        PlayerPrefs.SetInt("DragonFrame", 1);
        PlayerPrefs.SetInt("HeroFrame", 1);
        PlayerPrefs.SetInt("MetallFrame", 1);
        PlayerPrefs.SetInt("GoldenFrame", 1);
        PlayerPrefs.SetInt("CarpetFrame", 1);
        PlayerPrefs.SetInt("GotFrame", 1);
    }

    void Update() {
        if (started)
        {
            startTime += Time.deltaTime;
            if (starting)
            {
                
                player.transform.Translate(moveUnitsPerSecond * startTime * 0.009f);
                return;
            }
            if (!finished)
            {
                minutes = (int)Mathf.Floor((startTime - 4) / 60);
                seconds = (int)((startTime - 4) % 60);
                timeText.text = minutes.ToString("00") + ":" + seconds.ToString("00");

                //изменение скорости с количеством очков
                float speed = (GetScore() / 1000f) + 2f;
                float speedBonus = (bonus.isSpeedBonus() && bonus.IsActive()) ? speed * bonus.Multiplier : 0;
                speedBonus = (bonus.Name.Equals(BonusType.Fast)) ? speedBonus : -speedBonus;
                player.transform.Translate((speed + speedBonus) * moveUnitsPerSecond * Time.deltaTime);
                if (Time.time >= _nextTime)
                {
                    SetCarmaOnUpdate(-1.2f);  // we need constantly decrease the carma with time
                    _nextTime += 0.5f / speed;
                }

                if (!_isBadCarma && GetCarmaState().Equals(CarmaStates.Bad))
                {
                    _isBadCarma = true;
                    badCarmaAlert.GetComponent<Animator>().enabled = true;
                }

                if (_isBadCarma && !GetCarmaState().Equals(CarmaStates.Bad))
                {
                    _isBadCarma = false;
                    badCarmaAlert.GetComponent<Animator>().enabled = false;
                    badCarmaAlert.GetComponent<Image>().color = new Color(1, 1, 1, 0);
                }

                // разблокрирование сверхчеловека
                if (startTime - _timeLastFail >= 603)
                {
                    if (_pauseScript.pauseCount < 1)
                    {
                        GooglePlayActions.UnlockAchievement(GooglePlayActions.superhumanAchiev);
                        PlayerPrefs.SetInt("HeroFrame", 1);
                    }
                    else
                    {
                        GooglePlayActions.UnlockAchievement(GooglePlayActions.davidBlaineAchiev);
                        PlayerPrefs.SetInt("DragonFrame", 1);
                    }
                }

                // spawn help text
                if ((Time.time - _timeLastBursted > 10) && (Time.time - _timeLastHelpSpawn > 5))
                {
                    StartCoroutine(_gridManager.SpawnHelpText());
                    _timeLastHelpSpawn = Time.time;
                }

                // разблокирование апатии
                    if (startTime - _timeLastFail >= 30 && startTime - _timeLastBursted >= 30)
                {
                    GooglePlayActions.UnlockAchievement(GooglePlayActions.apathyAchiev);
                }
            }
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (helpButton.GetComponent<OpenHelp>().IsOpened())
            {
                helpButton.GetComponent<OpenHelp>().OnClick();
            }
            else if (menuController.IsOpened())
            {
                backButtonIngame.GetComponent<UIButton>().OnClick.Invoke();
            }
            else
            {
                Application.Quit();
            }
        }
    }


    public bool IsFreeBonusActive()
    {
        return freeBonus.IsActive ();
    }

    public void OnLineDeleted()
    {
        _gridManager.deleteGrid();
        _gridManager.createGrid(6, 1);
    }

    public int GetScore(){
        return this._score;
    }

    private float GetCarma(){
        return this._carma;
    }

    public void SetScore(int score){
        _score += score;
        _score = (_score < 0) ? 0 : _score;
        scoreText.GetComponent<Text>().text = GetScore().ToString();
    }

    private void SetCarmaOnUpdate(float carma){
        SetCarma (carma);
        // we need to increase bad bubbles spawn % if carma is bad and decrease in other cases
        if (GetCarmaState ().Equals (CarmaStates.Bad) && (_gridManager.GetBadSpawnPerc() < 80)) {
            _gridManager.SetBadSpawnPerc((Math.Abs (this._carma) % 50));
        } else if (!GetCarmaState ().Equals (CarmaStates.Bad) && (_gridManager.GetBadSpawnPerc() > 10)) {
            _gridManager.SetBadSpawnPerc(-(Math.Abs (this._carma) % 50));
        }
    }

    public void SetCarma(float carma){
        int bonusCarma = (bonus.Name.Equals(BonusType.Carma) && bonus.IsActive()) ? (int)(carma * bonus.Multiplier) : 0;  
        this._carma += carma + bonusCarma;
        if (this._carma < -100) { 
            this._carma = -100;
            if (!this.finished)
            {
                StartCoroutine("FinishGame");
            }
            return;
        };
        if (this._carma > 100) { this._carma = 100; };

        // update carma progress bar
        float amount = Math.Abs (GetCarma () / 100f);
        if (GetCarma() > 0) {
            carmaRightImage.fillAmount = amount;
        }
        else {
            carmaLeftImage.fillAmount = amount;
        }
    }

    IEnumerator FinishGame(){
        finished = true;
        isRecord = false;
        List<string> recordAnnotation = new List<string>();
        try
        {
            if (GetScore() > PlayerPrefs.GetInt("Score", 0))
            {
                isRecord = true;
                PlayerPrefs.SetInt("Score", GetScore());
                recordAnnotation.Add("Score: " + GetScore());
                ActivateRecordScreen();
                GooglePlayActions.ReportResult(GooglePlayActions.scoreLeaderboardId, GetScore());
            }
            if (startTime - 3 > PlayerPrefs.GetInt("Time", 0))
            {
                isRecord = true;
                PlayerPrefs.SetInt("Time", (int)(Time.timeSinceLevelLoad - 3));
                recordAnnotation.Add("Time: " + minutes.ToString("00") + ":" + seconds.ToString("00"));
                ActivateRecordScreen();
                GooglePlayActions.ReportResult(GooglePlayActions.timeLeaderboardId, (long)(Time.timeSinceLevelLoad - 3) * 1000);
            }
            if (_badBubblesBurst > PlayerPrefs.GetInt("BadBubblesCount", 0))
            {
                isRecord = true;
                PlayerPrefs.SetInt("BadBubblesCount", _badBubblesBurst);
                recordAnnotation.Add("Bad bubbles: " + _badBubblesBurst);
                ActivateRecordScreen();
                GooglePlayActions.ReportResult(GooglePlayActions.bubblesCountLeaderboardId, _badBubblesBurst);
            }
            // достижение за прохождение первой трассы
            if (!PlayerPrefs.HasKey("Score"))
            {
                GooglePlayActions.UnlockAchievement(GooglePlayActions.pioneerAchiev);
                PlayerPrefs.SetInt("PinkFrame", 1);
            }
            // достижение за скоростной слив игры
            if (startTime - 3 <= 19)
            {
                GooglePlayActions.UnlockAchievement(GooglePlayActions.destrudoAchiev);
            }
            // достижение за 3000 очков
            if (GetScore() >= 3000)
            {
                GooglePlayActions.UnlockAchievement(GooglePlayActions.longLiveAchiev);
                PlayerPrefs.SetInt("KingFrame", 1);
            }

            //достижения за позицию в лидерборде
            GooglePlayActions.GetLeaderboardRank(GooglePlayActions.scoreLeaderboardId);
            GooglePlayActions.GetLeaderboardRank(GooglePlayActions.timeLeaderboardId);
            GooglePlayActions.GetLeaderboardRank(GooglePlayActions.bubblesCountLeaderboardId);

        }
        catch (Exception e){
            Debug.Log(e);
        }

        menuPanel.SetActive(true);
        backButtonIngame.SetActive(false);

        menuBackground.GetComponent<UIElement>().Show(false);
        menuPanel.GetComponent<UIElement>().Show(false);
        nextButton.SetActive(true);
       
        audioSource.volume = 0.1f;
        badCarmaAlert.GetComponent<Animator>().enabled = false;
        pauseButton.GetComponent<Button>().interactable = false;
        finishPanel.SetActive(true);
        buttonsPanel.SetActive(true);

        scoresText.text = GetScore().ToString();
        goodBubblesText.text = _goodBubblesBurst.ToString();
        badBubblesText.text = _badBubblesBurst.ToString();
        finishTimeText.text = minutes.ToString("00") + ":" + seconds.ToString("00");

        finishText.text = "the end";

        if (!isRecord)
        {
            //bool showAds = (UnityEngine.Random.Range(1, 5) == 1) ? true : false;
            //if (showAds)
            //{
            //    RequestInterstitialAds();
            //    Utilities.UpdatePrefs("WatchAdsCounter", 1);
            //}
            Utilities.UpdatePrefs("IsRecord", 0);
        }
        else {
            this.ActivateRecordScreen();
            Utilities.UpdatePrefs("IsRecord", 1);
            yield return new WaitForSeconds(1f);

            if (Permission.HasUserAuthorizedPermission(Permission.Camera))
            {
                SceneManager.LoadScene("PhotoScene");
            }
            else
            {
                AndroidRuntimePermissions.Permission result = AndroidRuntimePermissions.RequestPermission("android.permission.CAMERA");
                if (result == AndroidRuntimePermissions.Permission.Granted)
                    SceneManager.LoadScene("PhotoScene");
            }
        }
            
        if (recordAnnotation.Count > 0) {
            PlayerPrefs.SetString ("RecordAnnotation", String.Join("\n", recordAnnotation.ToArray()));
        }
        
    }

    private void ActivateRecordScreen(){
        photoButton.SetActive (true);
        highScoreImage.SetActive (true);
        finishText.text = "A new record!";
    }

    public void RequestInterstitialAds()
    {
        if (Advertisement.IsReady("video"))
        {
            Advertisement.Show("video");
        }

    }

    private void ChangeScore(Vector3 whereToSpawnAdder){
        CarmaStates carmaState = GetCarmaState();
        int scoreToAdd = 0;
        if (carmaState.Equals(CarmaStates.Good))            
            scoreToAdd = 7;
        else if (carmaState.Equals(CarmaStates.Neutral))
            scoreToAdd = 5;
        else
            scoreToAdd = 10;
        
        int scoreBonus = (bonus.isScoreBonus() && bonus.IsActive()) ? (int)(scoreToAdd * bonus.Multiplier) : 0;
        scoreBonus = (bonus.Name.Equals (BonusType.Slow)) ? -scoreBonus : scoreBonus;
        SetScore (scoreToAdd + scoreBonus);
        ScoreAnimate (scoreToAdd + scoreBonus, whereToSpawnAdder);
    }

    private void ChangeCarma(){
        CarmaStates carmaState = GetCarmaState();
        int carmaToCange = 0;
        if (carmaState.Equals(CarmaStates.Good))            
            carmaToCange = 2;
        else if (carmaState.Equals(CarmaStates.Neutral))
            carmaToCange = 3;
        else
            carmaToCange = 4;
        SetCarma (carmaToCange);
    }

    private CarmaStates GetCarmaState(){
        if (GetCarma () > 50)
            return CarmaStates.Good;
        if (GetCarma () > -50)
            return CarmaStates.Neutral;
        return CarmaStates.Bad;
    }

    public void BurstGoodBubble(GameObject obj, Sprite[] burstSprites = null){
        //set on of the "bursted" sprites to good bubble
        if (starting) {
            return;
        }

        if (burstSprites == null)
        {
            int spriteNumber = UnityEngine.Random.Range(0, goodBubblesSprites.Length);
            obj.GetComponent<SpriteRenderer>().sprite = goodBubblesSprites[spriteNumber];

            if (spriteNumber == 0)
            {
                _vangaCookiesBursted += 1;
                //Debug.Log(_vangaCookiesBursted);
            }

            if (_vangaCookiesBursted == 12)
            {
                GooglePlayActions.UnlockAchievement(GooglePlayActions.vangaCookiesAchiev);
            }
        }
        else
        {
            obj.GetComponent<SpriteRenderer>().sprite = burstSprites[UnityEngine.Random.Range(0, burstSprites.Length)];
        }

        //obj.GetComponent<SpriteRenderer> ().sprite = (burstSprites == null) ? goodBubblesSprites[UnityEngine.Random.Range(0, goodBubblesSprites.Length)] : burstSprites[UnityEngine.Random.Range (0, burstSprites.Length)] ;
        if (!bool.Parse(PlayerPrefs.GetString("isSoundOff", "false"))) {
            obj.GetComponent<AudioSource> ().clip = goodBubbleSounds [UnityEngine.Random.Range (0, goodBubbleSounds.Length)];
            obj.GetComponent<AudioSource> ().Play ();
        }

        if (burstSprites == null)
        {
            Destroy(obj.GetComponent("GoodBubbleClick"));
        }
        else
        {
            Destroy(obj.GetComponent("BonusBubbleClick"));
        }

        ChangeScore (obj.transform.position);
        ChangeCarma ();

        if ((Time.time - _timeLastBursted) < 0.5) {
            _burstedInMultiTouch += 1;
        } else {
            _burstedInMultiTouch = 0;
        }
        // открыть достижение ловкач
        if (_burstedInMultiTouch > 2) {
            GooglePlayActions.UnlockAchievement (GooglePlayActions.tricksterAchiev);
        }

        _goodBubblesBurst += 1;
        _timeLastBursted = Time.time;

        if (obj.GetComponentInChildren<CloseHelp>())
        {
            obj.GetComponentInChildren<CloseHelp>().onHelpClose();
        }
    }

    public void BurstBonusBubble(GameObject obj){

        if (obj.name.StartsWith ("Free")) {
            BurstGoodBubble(obj, freeBubblesSprites);
            freeBonus.Activate ();
        } else if (obj.name.StartsWith ("Fire") || obj.name.StartsWith ("Poison") || obj.name.StartsWith ("Ghost")) {
            visualBonus.Activate ();
            if (obj.name.StartsWith ("Fire")) {
                visualBonus.Name = BonusType.Fire;
                BurstGoodBubble(obj, fireBubblesSprites);
            } else if (obj.name.StartsWith ("Poison")) {
                visualBonus.Name = BonusType.Poison;
                BurstGoodBubble(obj, poisonBubblesSprites);
            } else {
                visualBonus.Name = BonusType.Ghost;
                BurstGoodBubble(obj, ghostBubblesSprites);
            }
        }
        else {
            float bonusValue = (float)Math.Round(UnityEngine.Random.Range(0.3f, 0.8f), 1);
            BonusType type;
            if (obj.name.StartsWith ("Fast")) {
                BurstGoodBubble(obj, fastBubblesSprites);
                type = BonusType.Fast;
                bonusText.color = new Color (1, 0.7f, 0);
                bonusTimerImage.color = new Color (1, 0.7f, 0);
            } else if (obj.name.StartsWith ("Slow")) {
                BurstGoodBubble(obj, slowBubblesSprites);
                type = BonusType.Slow;
                bonusText.color = Color.green;
                bonusTimerImage.color = Color.green;
            } else if (obj.name.StartsWith ("Carma")) {
                BurstGoodBubble(obj, carmaBubblesSprites);
                type = BonusType.Carma;
                bonusText.color = Color.magenta;
                bonusTimerImage.color = Color.magenta;
            }
        else {
                BurstGoodBubble(obj, scoreBubblesSprites);
                type = BonusType.Score;
                bonusText.color = Color.yellow;
                bonusTimerImage.color = Color.yellow;
            } 
            bonus = new Bonus (type, bonusValue, 10);
        }
        if (obj.GetComponentInChildren<CloseHelp>())
        {
            obj.GetComponentInChildren<CloseHelp>().onHelpClose();
        }
    }

    public void BurstBadBubble(GameObject obj){
        Destroy (obj.GetComponent<Animator>());
        obj.GetComponent<SpriteRenderer> ().sprite = badBubblesSprites[badBubblesSprites.Length - 1];
        if (!bool.Parse(PlayerPrefs.GetString("isSoundOff", "false")))
        {
            obj.GetComponent<AudioSource>().clip = badBubbleSounds[UnityEngine.Random.Range(0, 2)];
            obj.GetComponent<AudioSource>().Play();
        }
        Destroy (obj.GetComponent<EventTrigger>());

        _timeLastFail = startTime;
        _badBubblesBurst += 1;

        if (_badBubblesBurst == 1 && !PlayerPrefs.HasKey("FirstBadBubble")) {
            PlayerPrefs.SetString ("FirstBadBubble", "false");
            GooglePlayActions.UnlockAchievement (GooglePlayActions.scrapThisAchiev);
        }
    }

    public void FrightenFly(GameObject obj) {
        if (starting) {
            return;
        }

        _timeLastFail = startTime;
        _fliesCount += 1;
        if (!bool.Parse(PlayerPrefs.GetString("isSoundOff", "false")))
        {
            obj.GetComponent<AudioSource>().clip = flySound;
            obj.GetComponent<AudioSource>().Play();
        }
        // разблокирование достижений на количество мух
        switch (_fliesCount)
        { 
            case 13:
                GooglePlayActions.UnlockAchievement (GooglePlayActions.lordAchiev);
                break;
            case 21:
                GooglePlayActions.UnlockAchievement (GooglePlayActions.flyBaronAchiev);
                break;
            case 34:
                GooglePlayActions.UnlockAchievement (GooglePlayActions.flyOnTheWallAchiev);
                break;
        }
        obj.GetComponent<Animator> ().SetTrigger ("Clicked");
        obj.GetComponent<RandomFlying> ().enabled = true;

        if (_fliesCount == 1 && !PlayerPrefs.HasKey("FirstFly")) {
            PlayerPrefs.SetString ("FirstFly", "false");
            GooglePlayActions.UnlockAchievement (GooglePlayActions.soLovelyAchiev);
        }
    }

    private void ScoreAnimate(int scoreToAdd, Vector3 whereToSpawnAdder){
        // define how much stars we need to spawn
        int starsNumber = scoreToAdd / 3;
        starsNumber = (starsNumber < 1) ? 1 : starsNumber;
        for (int i = 0; i < starsNumber; i++){
            float size = UnityEngine.Random.Range(100, 170);
            StartCoroutine(GenerateStarCoroutine(whereToSpawnAdder, new Vector3(size, size, size), 0.05f));
        }
        GameObject scoreAdder = Instantiate(scoreAdderImage, whereToSpawnAdder,  Quaternion.identity) as GameObject;
        scoreAdder.transform.SetParent(GameObject.Find("Canvas").transform, false);
        scoreAdder.transform.SetAsFirstSibling();
        scoreAdder.transform.position = whereToSpawnAdder;
        scoreAdder.GetComponent<ScoreAdder>().AddScore(scoreToAdd);
        Destroy (scoreAdder, 0.7f);
    }


    private GameObject GenerateStar(Vector3 pos, Vector3 size)
    {
        GameObject star = Instantiate(starObject, pos, Quaternion.identity) as GameObject;
        star.transform.SetParent(GameObject.Find("Canvas").transform, false);
        star.transform.SetAsFirstSibling();
        star.transform.localScale = size;
        star.transform.position = pos;
        Destroy(star, 1f);
        return star;
    }

    IEnumerator GenerateStarCoroutine(Vector3 pos, Vector3 size, float time)
    {
        yield return new WaitForSeconds(time);
        float xPos = UnityEngine.Random.Range(-5, 5f);
        float speed = UnityEngine.Random.Range(5f, 7f);
        float angle = UnityEngine.Random.Range(450f, 600f);
        Vector3 rotateDirection = (UnityEngine.Random.Range(0, 2) == 0) ? Vector3.back: Vector3.forward;
        GameObject star = GenerateStar(pos, size);
        float createTime = Time.time;
        while (star != null){
            star.transform.Rotate(rotateDirection, angle * Time.deltaTime);
            star.transform.Translate(new Vector3(xPos * Time.deltaTime, speed * Time.deltaTime, 0), Space.World);
            yield return new WaitForEndOfFrame();
        }
    }

    void OnApplicationPause(bool paused)
    {
        if (this.started && paused && !_pauseScript.IsPaused && !this.starting && !this.finished)
        {
            pauseButton.GetComponent<Button>().onClick.Invoke();
        }
    }
}
