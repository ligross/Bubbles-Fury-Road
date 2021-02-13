using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DoozyUI;

public class BackButtonController : MonoBehaviour {
    public GameObject settingsPanel;
    public GameObject configPanel;
    public Image backgroundImage;
    public PauseScript pauseScript;
    public UIToggle menuToggle;
    public GameObject photoButton;
    public GameObject nextButton;

    public Sprite menuOffSprite;


    public void OnClick(){
        if (settingsPanel.activeInHierarchy){
            settingsPanel.SetActive(false);
            if (photoButton != null)
            {
                photoButton.SetActive(true);
            }
            if (nextButton != null)
            {
                nextButton.SetActive(true);
            }
            configPanel.SetActive(false);
            backgroundImage.overrideSprite = menuOffSprite;
            menuToggle.IsOn = false;
        }
        else{
            backgroundImage.overrideSprite = menuOffSprite;
            if (pauseScript != null){
                pauseScript.Pause();
            }

        }
    }
}
