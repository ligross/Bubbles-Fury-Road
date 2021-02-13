using UnityEngine.UI;
using UnityEngine;
using System.IO;
using System;
using System.Linq;
using System.Collections.Specialized;
using System.Collections.Generic;
using DoozyUI;
using UnityEngine.SceneManagement;
using UnityEngine.Android;

public class PhotoPickerController : MonoBehaviour
{

    public GameObject previewPhotoPrefab;
    public GameObject previewPhotoContent;
	public GameObject fullsizePhotoPrefab;
	public GameObject fullsizePhotoContent;
    public GameObject deleteButton;
    public Button shareButton;
    public Toggle menuToggle;
    public GameObject settingsPanel;
    public GameObject configPanel;
    public GameObject previewPhotoPanel;
	public GameObject fullsizePhotoPanel;
    public GameObject photoButton;
    public GameObject nextButton;
    public GameObject backButton;
    public GameObject helpButton;
    public GameObject emptyGalleryDescription;
    public NativeSharing sharing;
    //private OrderedDictionary photos;
    //private List<string> fileList;
    //const string _path = "/storage/emulated/0/DCIM/bubbles_gallery";
    //const string _path = "/Users/ligross/Projects/Unity/bubbles_gallery";

    private List<GameObject> photoPicker = new List<GameObject>();
	List<GameObject> fullsizePhotoPicker = new List<GameObject>();
	List<string> photoNames = new List<string>();

	//swipe variables
	bool swiping;
	float startTime;
	float journeyLength;
	float speed = 0.4f;
	float initialPosition;
	int currentPhotoIndex;
    
    // Use this for initialization
    void Start()
    {      
		Time.timeScale = 1f;

        int photoSizeHeight = (int)(Screen.height);
        int photoSizeWidth = (int)(Screen.width);
        if (photoSizeHeight < 1920)
        {
            fullsizePhotoPrefab.GetComponent<LayoutElement>().preferredHeight = 1920;
            fullsizePhotoPrefab.GetComponent<LayoutElement>().preferredWidth = 1080;
            RectTransform rt = fullsizePhotoPrefab.GetComponent<RectTransform>();
            rt.sizeDelta = new Vector2(1080, 1920);
        }
        else
        {
            fullsizePhotoPrefab.GetComponent<LayoutElement>().preferredHeight = photoSizeHeight;
            fullsizePhotoPrefab.GetComponent<LayoutElement>().preferredWidth = photoSizeWidth;
            RectTransform rt = fullsizePhotoPrefab.GetComponent<RectTransform>();
            rt.sizeDelta = new Vector2(photoSizeWidth, photoSizeHeight);
        }

        //fullsizePhotoPanel.GetComponent<RectTransform>().sizeDelta = new Vector2(photoSizeWidth, photoSizeHeight);
        string _path = Application.persistentDataPath;
        string[] files = Directory.GetFiles(_path, "*.png");//loads only png files
        OrderedDictionary photos = ReadPhotos(files);

		foreach (string key in photos.Keys)
        {
			GameObject previewPhoto = Instantiate(previewPhotoPrefab);
			photoNames.Add(key);
			previewPhoto.GetComponent<PhotoPicker>().ind = photoNames.IndexOf(key);
            Texture2D tex = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, false);
            tex.LoadImage(photos[key] as byte[]);
            previewPhoto.GetComponent<RawImage>().texture = tex;
            previewPhoto.transform.SetParent(previewPhotoContent.transform, false);
            previewPhoto.GetComponent<LayoutElement>().preferredWidth = (200f / tex.height) * tex.width;
			photoPicker.Add(previewPhoto);



			GameObject fullsizePhoto = Instantiate(fullsizePhotoPrefab);
			fullsizePhoto.GetComponent<RawImage>().texture = tex;
            fullsizePhoto.transform.SetParent(fullsizePhotoContent.transform, false);
            //fullsizePhoto.GetComponent<RectTransform>().sizeDelta = new Vector2(tex.width, tex.height);
            //fullsizePhoto.GetComponent<LayoutElement>().preferredWidth = tex.width;
            //fullsizePhoto.GetComponent<LayoutElement>().preferredHeight = tex.height;
			fullsizePhotoPicker.Add(fullsizePhoto);

        }

        if (PlayerPrefs.HasKey("Score"))
        {
            photoButton.SetActive(true);
        }
        else
        {
            photoButton.SetActive(false);
        }


        if (photos.Count > 0)
        {
			fullsizePhotoPanel.GetComponent<ScrollRect>().horizontalNormalizedPosition = 0;
            deleteButton.SetActive(true);
            shareButton.interactable = true;
			fullsizePhotoPanel.SetActive(true);
			previewPhotoPanel.SetActive(true);
            //PickPhoto(photoNames.Last());
        }
        else
        {
			fullsizePhotoPanel.gameObject.SetActive(false);
        }
        //if(PlayerPrefs.HasKey("Score")){
        //    photoButton.SetActive(true);
        //}
        if (photos.Count < 1)
        {
            if (PlayerPrefs.HasKey("Score"))
            {
                if (Permission.HasUserAuthorizedPermission(Permission.Camera))
                {
                    emptyGalleryDescription.GetComponent<Text>().text = "You do not have photo records yet, just press photo button at the top left to get one right now!";
                }
                else
                {
                    emptyGalleryDescription.GetComponent<Text>().text = "You do not have photo records yet since you didn't allow the camera usage. You can change it right now by pressing the photo button at the top left!";
                }
            }
            else
            {
                emptyGalleryDescription.GetComponent<Text>().text = "Your photo records will be here, but so far your gallery is empty";
            }
     
            emptyGalleryDescription.SetActive(true);
        }
        BeforePhotoScene();
    }

	private void Update()
	{
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (helpButton.GetComponent<OpenHelp>().IsOpened())
            {
                helpButton.GetComponent<OpenHelp>().OnClick();
            }
            else if (menuToggle.isOn)
            {
                backButton.GetComponent<Button>().onClick.Invoke();
            }
            else
            {
                SceneManager.LoadScene("PlayScene");
            }
        }

        if (swiping){
            float distCovered = (Time.time - startTime) * (speed / photoNames.Count);

            // Fraction of journey completed = current distance divided by total distance.
            float fracJourney = distCovered / journeyLength;

            // Distance moved = time * speed
            if (journeyLength > 0)
            {
                if (fullsizePhotoPanel.GetComponent<ScrollRect>().horizontalNormalizedPosition + fracJourney >= initialPosition + journeyLength)
                {
                    fullsizePhotoPanel.GetComponent<ScrollRect>().horizontalNormalizedPosition = initialPosition + journeyLength;
                    swiping = false;
                    return;
                }
                // Set our position as a fraction of the distance between the markers
                fullsizePhotoPanel.GetComponent<ScrollRect>().horizontalNormalizedPosition += fracJourney;
            }


            if (journeyLength < 0)
            {
                if (fullsizePhotoPanel.GetComponent<ScrollRect>().horizontalNormalizedPosition + fracJourney <= initialPosition + journeyLength)
                {
                    fullsizePhotoPanel.GetComponent<ScrollRect>().horizontalNormalizedPosition = initialPosition + journeyLength;
                    swiping = false;
                    return;
                }
                // Set our position as a fraction of the distance between the markers
                fullsizePhotoPanel.GetComponent<ScrollRect>().horizontalNormalizedPosition += fracJourney;
            }


			if (fullsizePhotoPanel.GetComponent<ScrollRect>().horizontalNormalizedPosition > 1.001){
				fullsizePhotoPanel.GetComponent<ScrollRect>().horizontalNormalizedPosition = 1;
				swiping = false;
			}
			if(fullsizePhotoPanel.GetComponent<ScrollRect>().horizontalNormalizedPosition < - 0.001)
            {
                fullsizePhotoPanel.GetComponent<ScrollRect>().horizontalNormalizedPosition = 0;
                swiping = false;
            }
		}
	}


    private OrderedDictionary ReadPhotos(string[] filelist)
    {
        OrderedDictionary photosDict = new OrderedDictionary();
        foreach (string file in filelist.Reverse())
        {
			photosDict.Add(file, (File.ReadAllBytes(file)));
        }

		return photosDict;
    }
    
    public void PickPhoto(int ind)
    {
		if(ind > currentPhotoIndex){
			SwipeLeft(Math.Abs(currentPhotoIndex - ind));
		}
		else if (ind < currentPhotoIndex){
			SwipeRight(Math.Abs(currentPhotoIndex - ind));
		}
        //_pickedImage = name;
        //Texture2D tex = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, false);
        //tex.LoadImage(photos[name] as byte[]);
        //float videoRatio = (float)tex.width / (float)tex.height;
        //photoImage.GetComponent<AspectRatioFitter>().aspectRatio = videoRatio;
        //photoImage.texture = tex;
    }

    public void SwipePhoto(Vector2 value)
    {
        if (fullsizePhotoPanel.GetComponent<ScrollRect>().horizontalNormalizedPosition > value.y)
        {
            SwipeLeft();
        }
        else
        {
            SwipeRight();
        }
    }


    public void SwipeLeft(int itemsToMove = 1){
		if (!swiping){
			float positionToMove = (1f / photoNames.Count) * itemsToMove;
			float shift = 1f / (photoNames.Count - 1);
			journeyLength = positionToMove * (1f + shift);
            
			if (fullsizePhotoPanel.GetComponent<ScrollRect>().horizontalNormalizedPosition + journeyLength >= (1f + shift))
            {
                return;
                
            }
			if (currentPhotoIndex >= photoNames.Count){
				return;
			}
			startTime = Time.time;
			initialPosition = fullsizePhotoPanel.GetComponent<ScrollRect>().horizontalNormalizedPosition;
			swiping = true;
			currentPhotoIndex += itemsToMove;
			Debug.Log(currentPhotoIndex);
		}
    }

	public void SwipeRight(int itemsToMove = 1)
    {
		if (!swiping)
        {
			float positionToMove = (1f / photoNames.Count) * itemsToMove;
			float shift = 1f / (photoNames.Count - 1);
			journeyLength = - positionToMove * (1f + shift);
			if (fullsizePhotoPanel.GetComponent<ScrollRect>().horizontalNormalizedPosition - journeyLength <= -shift)
            {
                return;
            }
			if (currentPhotoIndex <= 0){
				return;
			}
			currentPhotoIndex -= itemsToMove;
			startTime = Time.time;
			initialPosition = fullsizePhotoPanel.GetComponent<ScrollRect>().horizontalNormalizedPosition;
			swiping = true;
			Debug.Log(currentPhotoIndex);
        }
    }
    

    public void DeletePhoto()
    {
		File.Delete(photoNames[currentPhotoIndex]);
		photoNames.RemoveAt(currentPhotoIndex);
		Destroy((UnityEngine.Object)photoPicker[currentPhotoIndex]);
		photoPicker.RemoveAt(currentPhotoIndex);
		Destroy((UnityEngine.Object)fullsizePhotoPicker[currentPhotoIndex]);
		fullsizePhotoPicker.RemoveAt(currentPhotoIndex);

		if (photoNames.Count > 0)
        {
			float shift = 1f / (photoNames.Count - 1);
			fullsizePhotoPanel.GetComponent<ScrollRect>().horizontalNormalizedPosition = 0;
			currentPhotoIndex = 0;
			for (int i = 0; i < photoNames.Count; i++){
				photoPicker[i].GetComponent<PhotoPicker>().ind = i;
			}
        }
        else
        {
            deleteButton.SetActive(false);
            shareButton.interactable = false;
			fullsizePhotoPanel.SetActive(false);
            previewPhotoPanel.SetActive(false);
            emptyGalleryDescription.GetComponent<Text>().text = "You do not have photo records yet, just press photo button at the top left to get one right now!";
            Color color = emptyGalleryDescription.GetComponent<Text>().color;
            color.a = 1;
            emptyGalleryDescription.GetComponent<Text>().color = color;
            emptyGalleryDescription.SetActive(true);
            emptyGalleryDescription.GetComponent<Animator>().enabled = true;
        }
    }

    public void ShareImage()
    {
		//sharing.screenshotPath = photoNames[currentPhotoIndex];
        //sharing.Share();
        new NativeShare().AddFile(photoNames[currentPhotoIndex]).SetSubject("My new record! #bubblesfuryroad").SetText("My new record! #bubblesfuryroad").Share();
    }


    public void MenuOnClick(){
		if (photoNames != null && photoNames.Count > 0){
            deleteButton.SetActive(!menuToggle.isOn);
            shareButton.interactable = !menuToggle.isOn;
			previewPhotoPanel.SetActive(!menuToggle.isOn);
        }
        else {
            deleteButton.SetActive(false);
            shareButton.interactable = false;
        }
        if (PlayerPrefs.HasKey("Score"))
        {
            Debug.Log("Display photo button");
            photoButton.SetActive(!menuToggle.isOn);
        }
        else
        {
            photoButton.SetActive(false);
        }
        shareButton.gameObject.SetActive(!menuToggle.isOn);
        settingsPanel.SetActive(menuToggle.isOn);
        nextButton.SetActive(!menuToggle.isOn);
        backButton.SetActive(menuToggle.isOn);
        configPanel.SetActive(menuToggle.isOn);

    }

    public void BeforePhotoScene(){
        PlayerPrefs.SetInt("IsRecord", 0);
    }

}
