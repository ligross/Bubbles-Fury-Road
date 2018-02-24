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
	public GameObject menuPanel;
    public GameObject menuBackground;
    public GameObject restartButton;

	public GameObject pauseText;
	public GameObject bonusAds;
	public GameObject adsButton;

    public int pauseCount;

    public void Pause()
    {
        _isPaused = !_isPaused;
		startFinishPanel.SetActive (_isPaused);
        menuPanel.SetActive (_isPaused);
        menuBackground.SetActive(_isPaused);
        pauseText.SetActive (_isPaused);
		bonusAds.SetActive (_isPaused);
        restartButton.SetActive(_isPaused);

        if (_isPaused)
        {
            // pause the game/physic
            pauseCount += 1;
            menuPanel.GetComponent<UIElement>().Show(false);
            menuBackground.GetComponent<UIElement>().Show(false);

            if (adsButton.GetComponent<AdmobController>().isAdsAvailable) 
				adsButton.GetComponent<Button>().interactable = true;
            GameManager.Instance.SetScore(-100);
            GetComponent<Image>().sprite = pausedSprite;
            Time.timeScale = 0.0f;
            GameManager.Instance.audioSource.Pause();
        }
        else
        {
            // resume
            menuPanel.GetComponent<UIElement>().Hide(false);
            menuBackground.GetComponent<UIElement>().Hide(false);
            GetComponent<Image>().sprite = unpausedSprite;
			configController.RollUpConfig ();
            Time.timeScale = 1.0f;
            GameManager.Instance.audioSource.UnPause();
        }
    }
}
