using UnityEngine;
using System.Collections;
using System.IO;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Android;
using System;
using UnityEngine.SceneManagement;

public class WebCamPhotoCamera : MonoBehaviour 
{
	private WebCamTexture webCamTexture;

	public RectTransform imageParent;
	public Text annotationText;

	public GameObject photoButton;
    public GameObject nextButton;
    public GameObject backButton;
    public Toggle menuToggle;
    public GameObject backgroundPhotoButton;

    public GameObject framePanel;
    public GameObject menuPanel;
    public GameObject newRecord;

    WebCamDevice cameraDevice;

	// Image rotation
	Vector3 rotationVector = new Vector3(180f, 180f, 0f);

	// Image uvRect
	Rect defaultRect = new Rect(0f, 0f, 1f, 1f);
	Rect fixedRect = new Rect(0f, 1f, 1f, -1f);

	// Image Parent's scale
	Vector3 defaultScale = new Vector3(1f, 1f, 1f);
	Vector3 fixedScale = new Vector3(-1f, 1f, 1f);

	private bool _isPhotoTaken;
    private bool _isCameraReady;
    private byte[] _bytes;

    void Start()
    {
        if (WebCamTexture.devices.Length == 0)
        {
            Debug.Log("No devices cameras found");
            return;
        }
        try
        {
            cameraDevice = WebCamTexture.devices[1];
        }
        catch
        {
            cameraDevice = WebCamTexture.devices[0];
        }

        webCamTexture = new WebCamTexture(cameraDevice.name);
        webCamTexture.filterMode = FilterMode.Trilinear;
        //backCameraTexture = new WebCamTexture(backCameraDevice.name);

        GetComponent<RawImage>().texture = webCamTexture;

        webCamTexture.Play();
        //GetComponent<RawImage> ().SetNativeSize ();

        annotationText.text = PlayerPrefs.GetString("RecordAnnotation", "");
    }
	

	void Update()
	{
		if (!_isPhotoTaken) {
			// Skip making adjustment for incorrect camera data
			if (webCamTexture.width < 100) {
				Debug.Log ("Still waiting another frame for correct info...");
				return;
			}
            if (!_isCameraReady)
            {
                StartCoroutine("EnablePhotoTaking");
            }

			// Rotate image to show correct orientation 
			rotationVector.z = -webCamTexture.videoRotationAngle;
			GetComponent<RawImage> ().rectTransform.localEulerAngles = rotationVector;

			// Set AspectRatioFitter's ratio
			float videoRatio = (float)webCamTexture.width / (float)webCamTexture.height;
            GetComponent<AspectRatioFitter> ().aspectRatio = videoRatio;

			// Unflip if vertically flipped
			GetComponent<RawImage> ().uvRect = 
			webCamTexture.videoVerticallyMirrored ? fixedRect : defaultRect;

			// Mirror front-facing camera's image horizontally to look more natural
			imageParent.localScale = cameraDevice.isFrontFacing ? fixedScale : defaultScale;
		}
	}

    private IEnumerator EnablePhotoTaking()
    {
        yield return new WaitForSeconds(2f);
        _isCameraReady = true;
        photoButton.GetComponent<Button>().interactable = true;
        nextButton.GetComponent<Button>().interactable = true;
        backButton.GetComponent<Button>().interactable = true;
        backgroundPhotoButton.GetComponent<Button>().interactable = true;
        menuToggle.interactable = true;
    } 

    //public void TakePhoto()
    //{
    //    framePanel.SetActive(false);
    //    acceptPanel.SetActive(true);
    //    photoButton.SetActive(false);
    //    screenAcceptButton.interactable = false;
    //}

    public void TakePhoto()
    {
        if (!_isPhotoTaken && _isCameraReady)
        {
            StartCoroutine("TakeSnapshot");
        }  	
    }


    private IEnumerator TakeSnapshot()
    { 
    	framePanel.SetActive (false);
        newRecord.SetActive(false);
        menuPanel.SetActive(false);
    	yield return new WaitForEndOfFrame();

		Texture2D tex = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, false);

    	tex.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0);
    	tex.Apply ();
        //Encode texture into PNG
    	_isPhotoTaken = true;
    	webCamTexture.Stop ();

    	_bytes = tex.EncodeToPNG();
    	Destroy(tex);

    	photoButton.SetActive (false);
        menuPanel.SetActive(true);
        //screenAcceptButton.interactable = false;
        //GetComponent<RawImage> ().texture = tex;

        AcceptPhoto();
    }

    public void ExitScene()
	{
		if(webCamTexture != null)
			webCamTexture.Stop();

		//yield return new WaitForSeconds(0.2f);

		if(webCamTexture != null)
			Destroy(webCamTexture);

		//yield return new WaitForSeconds(0.2f);

		SceneManager.LoadScene("GalleryScene");
	}

    private void SavePhoto()
    {
        string _path = Application.persistentDataPath;
        //const string _path = "/storage/emulated/0/DCIM/bubbles_gallery";
        //string _path = "/Users/ligross/Projects/Unity/bubbles_gallery";

        Directory.CreateDirectory(_path);
        File.WriteAllBytes(_path + string.Format("/{0}.png", DateTime.Now.ToString("yyyyMMddHHmmss")), _bytes);

        //ачивка за первое фото
        switch (Directory.GetFiles(_path, "*.png").Length)
        {
		case 1:
			GooglePlayActions.UnlockAchievement (GooglePlayActions.narcissistAchiev);
            PlayerPrefs.SetInt("PinkFrame", 1);
            break;
		case 5:
			GooglePlayActions.UnlockAchievement (GooglePlayActions.discordantAchiev);
			break;
		case 8:
			GooglePlayActions.UnlockAchievement (GooglePlayActions.tetractisAchiev);
			break;
        }
    }

	public void AcceptPhoto(){
        SavePhoto();
		ExitScene();
	}
		
}