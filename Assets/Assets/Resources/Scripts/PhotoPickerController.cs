using UnityEngine.UI;
using UnityEngine;
using System.IO;
using System;
using System.Linq;
using System.Collections.Specialized;
using System.Collections.Generic;

public class PhotoPickerController : MonoBehaviour {

	public GameObject photoPrefab;
	public GameObject photoContent;
    public GameObject photoPlace;
    public GameObject deleteButton;

	private RawImage photoImage;
	private OrderedDictionary  photos;
    private string _pickedImage;
    private const string _path = "/storage/emulated/0/DCIM/bubbles_gallery";
    private Dictionary <string, GameObject> photoPicker = new Dictionary<string, GameObject>();

	// Use this for initialization
	void Start () {
		photoImage = photoPlace.GetComponent<RawImage>();

		string[] fileList = Directory.GetFiles( _path, "*.png");//loads only png files
		photos = ReadPhotos(fileList);

		foreach (string key in photos.Keys) {
			Debug.Log (key);
			GameObject photo = Instantiate(photoPrefab);
            photoPicker[key] = photo;
            photo.GetComponent<PhotoPicker> ().name = key;
			Texture2D tex = new Texture2D(Screen.width, Screen.height - 300, TextureFormat.RGB24, false);
			tex.LoadImage(photos[key] as byte[]);
			photo.GetComponent<RawImage>().texture = tex;
			photo.transform.SetParent(photoContent.transform, false);
		}

		if (photos.Count > 0) {
            deleteButton.SetActive(true);
            PickPhoto (fileList.Last());
		}
        else
        {
            photoPlace.gameObject.SetActive(false);
        }
	}

	private OrderedDictionary ReadPhotos(string[] filelist)
	{
        OrderedDictionary fred = new OrderedDictionary();
		foreach (string claire in filelist.Reverse())
		{
			fred.Add(claire, (File.ReadAllBytes(claire)));
		}

		return fred;
	}

	public void PickPhoto(string name){
        _pickedImage = name;
        Texture2D tex = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, false);
		tex.LoadImage(photos[name] as byte[]);
        float videoRatio = (float)tex.width / (float)tex.height;
        photoImage.GetComponent<AspectRatioFitter>().aspectRatio = videoRatio;
        photoImage.texture = tex;
    }

    public void DeletePhoto()
    {
        File.Delete(Path.Combine(_path, _pickedImage));
        photos.Remove(_pickedImage);
        Destroy(photoPicker[_pickedImage]);
        photoPicker.Remove(_pickedImage);

        if (photos.Keys.Count > 0)
        {
            string[] keys = new string[photos.Keys.Count];
            photos.Keys.CopyTo(keys, 0);
            PickPhoto(keys[0]);
        }
        else
        {
            deleteButton.SetActive(false);
            photoPlace.gameObject.SetActive(false);
        }
    }
}
