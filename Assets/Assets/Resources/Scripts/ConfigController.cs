using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ConfigController : MonoBehaviour {

    public bool isConfigPanelOpened;

    public GameObject сonfigPanel;

    public Button musicButton;
    public Button soundButton;

    public Image musicOffImage;
    public Image soundOffImage;

    public AudioSource musicSource;

    private bool _isMusicOff;
    private bool _isSoundOff;

    private void Start()
    {
        if (PlayerPrefs.HasKey("isMusicOff"))
        {
            _isMusicOff = bool.Parse(PlayerPrefs.GetString("isMusicOff"));
            if (_isMusicOff)
            {
                musicSource.Stop();
            }
            else
            {
                musicSource.Play();
            }
            musicOffImage.gameObject.SetActive(_isMusicOff);
        }
        if (PlayerPrefs.HasKey("isSoundOff"))
        {
            _isSoundOff = bool.Parse(PlayerPrefs.GetString("isSoundOff", "false"));
            soundOffImage.gameObject.SetActive(_isSoundOff);
        }
    }

    public void RollUpConfig() {
        if (сonfigPanel.activeSelf) {
            OnClick();
        }
    }

    public void OnClick() {
        isConfigPanelOpened = !isConfigPanelOpened;
        сonfigPanel.SetActive(isConfigPanelOpened);
    }

    public void OnMusicButtonClick()
    {
        _isMusicOff = !_isMusicOff;
        musicOffImage.gameObject.SetActive(_isMusicOff);
        PlayerPrefs.SetString("isMusicOff", _isMusicOff == true ? bool.TrueString : bool.FalseString);
        if (_isMusicOff)
        {
            musicSource.Stop();
        }
        else
        {
            musicSource.Play();
        }
    }

    public void OnSoundButtonClick()
    {
        _isSoundOff = !_isSoundOff;
        soundOffImage.gameObject.SetActive(_isSoundOff);
        PlayerPrefs.SetString("isSoundOff", _isSoundOff == true ? bool.TrueString : bool.FalseString);
    }

    public bool isSoundOff()
    {
        return _isSoundOff;
    }
}
