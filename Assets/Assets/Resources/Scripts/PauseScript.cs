using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DoozyUI;

public class PauseScript : MonoBehaviour {
    private bool _isPaused;

    public Sprite pausedSprite;
    public Sprite unpausedSprite;

    public GameObject startFinishPanel;
	public ConfigController configController;
    public UIElement menuPanel;
	public UIElement menuBackground;
    public GameObject restartButton;
	public GameObject backButtonIngame;
	public GameObject settingsPanel;
	public GameObject configPanel;
	public Toggle settingsToggle;
    public GameObject scoreDeduct;

	public GameObject pauseText;
	public GameObject adsButton;

	public Sprite settingsOffSprite;
    public AudioSource audioSource;
    public int pauseCount;

    public bool IsPaused{
        get
        {
            return _isPaused;
        }
        set{
            _isPaused = value;
        }
    }

    IEnumerator VolOut()
    {
        while (audioSource.volume > 0)
        {
            audioSource.volume -= 0.005f;
            yield return null;
        }
    }

    IEnumerator VolIn()
    {
        while (audioSource.volume < 1)
        {
            audioSource.volume += 0.005f;
            yield return null;
        }
    }
	public void Pause()
    {
        _isPaused = !_isPaused;


        pauseText.SetActive (_isPaused);
        adsButton.SetActive (_isPaused);
        restartButton.SetActive(_isPaused);
		backButtonIngame.SetActive (_isPaused);
        if (_isPaused)
        {
            
            // pause the game/physic
            pauseCount += 1;
            StartCoroutine(VolOut());
			startFinishPanel.SetActive (true);
            menuPanel.GetComponent<UIElement>().Show(false);
            menuBackground.Show(false);
            int scoreToDeduct = 0;
            if (GameManager.Instance.GetScore() >= 50)
            {
                scoreToDeduct = -50;
                GameManager.Instance.SetScore(-50);
            }
            else
            {
                scoreToDeduct = - GameManager.Instance.GetScore();
                GameManager.Instance.SetScore(-GameManager.Instance.GetScore());
            }
            if (scoreToDeduct < 0)
            {
                scoreDeduct.GetComponent<Text>().text = scoreToDeduct.ToString();
                scoreDeduct.GetComponent<Canvas>().enabled = true;
                scoreDeduct.GetComponent<GraphicRaycaster>().enabled = true;
                scoreDeduct.SetActive(true);
                scoreDeduct.GetComponent<UIElement>().Show(true);
            }
            
            if (adsButton.GetComponent<AdmobController>().isAdsAvailable) 
				adsButton.GetComponent<Button>().interactable = true;
            GetComponent<Image>().sprite = pausedSprite;
            Time.timeScale = 0.0f;
            GameManager.Instance.audioSource.Pause();
            AudioListener.pause = true;
        }
        else
        {
            // resume
			Time.timeScale = 1.0f;
            StartCoroutine(VolIn());
			menuBackground.Hide(false);
            menuPanel.GetComponent<UIElement>().Hide(false);
            GetComponent<Image>().sprite = unpausedSprite;
			settingsPanel.SetActive (false);
			configPanel.SetActive (false);
			configController.RollUpConfig ();
            GameManager.Instance.audioSource.UnPause();
            AudioListener.pause = false;
            startFinishPanel.SetActive (false);
        }
    }
}
