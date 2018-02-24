using UnityEngine;
using System.Collections;
using System.IO;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;

public class WebCamPhotoCamera : MonoBehaviour 
{
	private WebCamTexture webCamTexture;
	private Quaternion baseRotation;

	public RectTransform imageParent;
	public Text annotationText;

	public GameObject acceptPanel;
	public GameObject photoButton;
	public GameObject framePanel;
    public Button screenAcceptButton;

	WebCamDevice frontCameraDevice;

	// Image rotation
	Vector3 rotationVector = new Vector3(180f, 180f, 0f);

	// Image uvRect
	Rect defaultRect = new Rect(0f, 0f, 1f, 1f);
	Rect fixedRect = new Rect(0f, 1f, 1f, -1f);

	// Image Parent's scale
	Vector3 defaultScale = new Vector3(1f, 1f, 1f);
	Vector3 fixedScale = new Vector3(-1f, 1f, 1f);

	private bool _isPhotoTaken;
    private byte[] _bytes;

	void Start() 
	{
		if (WebCamTexture.devices.Length == 0)
		{
			Debug.Log("No devices cameras found");
			return;
		}
		frontCameraDevice = WebCamTexture.devices[1];
		//WebCamDevice backCameraDevice = WebCamTexture.devices[0];

		webCamTexture = new WebCamTexture(frontCameraDevice.name);
		webCamTexture.filterMode = FilterMode.Trilinear;
		//backCameraTexture = new WebCamTexture(backCameraDevice.name);

		GetComponent<RawImage> ().texture = webCamTexture;

		webCamTexture.Play();
		//GetComponent<RawImage> ().SetNativeSize ();

		annotationText.text = PlayerPrefs.GetString ("RecordAnnotation", "");
	}
		

	void Update()
	{
		if (!_isPhotoTaken) {
			// Skip making adjustment for incorrect camera data
			if (webCamTexture.width < 100) {
				Debug.Log ("Still waiting another frame for correct info...");
				return;
			}

			// Rotate image to show correct orientation 
			rotationVector.z = -webCamTexture.videoRotationAngle;
			GetComponent<RawImage> ().rectTransform.localEulerAngles = rotationVector;

			// Set AspectRatioFitter's ratio
			float videoRatio = 
				(float)webCamTexture.width / (float)webCamTexture.height;
			GetComponent<AspectRatioFitter> ().aspectRatio = videoRatio;

			// Unflip if vertically flipped
			GetComponent<RawImage> ().uvRect = 
			webCamTexture.videoVerticallyMirrored ? fixedRect : defaultRect;

			// Mirror front-facing camera's image horizontally to look more natural
			imageParent.localScale = 
			frontCameraDevice.isFrontFacing ? fixedScale : defaultScale;
		}
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
    	StartCoroutine("TakeSnapshot");
    }


    private IEnumerator TakeSnapshot()
    { 
    	framePanel.SetActive (false);
    	yield return new WaitForEndOfFrame();

    	Texture2D tex = new Texture2D (Screen.width, Screen.height - 300, TextureFormat.RGB24, false);

    	tex.ReadPixels(new Rect(0, 300, Screen.width, Screen.height - 300), 0, 0);
    	tex.Apply ();
        //Encode texture into PNG
    	_isPhotoTaken = true;
    	webCamTexture.Stop ();

    	_bytes = tex.EncodeToPNG();
    	Destroy(tex);

    	acceptPanel.SetActive (true);
    	photoButton.SetActive (false);
        screenAcceptButton.interactable = false;
    	//GetComponent<RawImage> ().texture = tex;
    }

    private void ExitScene()
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
        string path = "/storage/emulated/0/DCIM/bubbles_gallery";
        Directory.CreateDirectory(path);
        File.WriteAllBytes(path + string.Format("/{0}.png", DateTime.Now.ToString("yyyyMMddHHmmss")), _bytes);

        //ачивка за первое фото
        switch (Directory.GetFiles(path, "*.png").Length)
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

    //public void AcceptPhoto()
    //{
    //    NPBinding.MediaLibrary.SaveScreenshotToGallery(SaveImageToGalleryFinished);
    //    ExitScene();
    //}

    //private void SaveImageToGalleryFinished(bool _saved)
    //{
    //    Debug.Log("Saved image to gallery successfully ? " + _saved);
    //}

    public void DeclinePhoto(){
		webCamTexture.Play ();
		_isPhotoTaken = false;
		framePanel.SetActive (true);
		photoButton.SetActive (true);
		acceptPanel.SetActive (false);
        screenAcceptButton.interactable = true;
    }
		
}