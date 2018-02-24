using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundPlayer : MonoBehaviour {

    public void Play()
    {
        if (!bool.Parse(PlayerPrefs.GetString("isSoundOff", "false")))
            GetComponent<AudioSource>().Play();
    }
}
