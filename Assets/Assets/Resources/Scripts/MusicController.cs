using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicController : MonoBehaviour {
	// Use this for initialization
	public AudioSource audioSource;

	void Start () {
		if (PlayerPrefs.HasKey("isMusicOff"))
		{
			bool _isMusicOff = bool.Parse(PlayerPrefs.GetString("isMusicOff"));
			if (_isMusicOff)
			{
				audioSource.enabled = false;
			}
			else
			{
				audioSource.enabled = true;
			}
		}
	}

}
