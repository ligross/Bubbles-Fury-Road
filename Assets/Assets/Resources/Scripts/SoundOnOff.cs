using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SoundOnOff : MonoBehaviour {
	bool soundOn = true;
	Image image;
	Sprite soundOnSprite;
	Sprite soundOffSprite;

	public AudioSource audioSource;

	public void Start(){
		image = this.GetComponent<Image>();
		soundOnSprite = Resources.LoadAll<Sprite> ("Sprites/sprite-interface")[12];
		soundOffSprite = Resources.LoadAll<Sprite> ("Sprites/sprite-interface")[21];
		soundOn = PlayerPrefs.GetInt("SoundOff") == 1 ? false : true;
		image.sprite = soundOn ? soundOnSprite : soundOffSprite;
		audioSource.enabled = soundOn;
	}

    public void onClick(){
		soundOn = !soundOn;
		audioSource.enabled = soundOn;
		image.sprite = soundOn ? soundOnSprite : soundOffSprite;
		PlayerPrefs.SetInt("SoundOff", !soundOn ? 1 : 0);
	} 
}
