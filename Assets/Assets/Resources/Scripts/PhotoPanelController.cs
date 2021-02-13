using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DoozyUI;
using UnityEngine.SceneManagement;

public class PhotoPanelController : MonoBehaviour
{
    public Toggle menuToggle;
    public Image menuBackground;

    public GameObject settingsPanel;
    public GameObject configPanel;
    public GameObject framePanel;
    public GameObject photoButton;
    public GameObject nextButton;
    public GameObject backButton;
    public GameObject helpButton;

    public Sprite settingsOnSprite;
    public Sprite settingsOffSprite;

    public GameObject newRecordText;

    public bool isPhotoTaken;

	public void Start()
	{
        if (PlayerPrefs.GetInt("IsRecord", 0) == 1)
        {
            StartCoroutine("AnimateNewRecord");
        }
        else {
            backButton.SetActive(false);
            nextButton.SetActive(true);
        }
	}

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (helpButton.GetComponent<OpenHelp>().IsOpened())
            {
                helpButton.GetComponent<OpenHelp>().OnClick();
            }
            else if (menuToggle.isOn)
            {
                backButton.GetComponent<Button>().onClick.Invoke();
            }
            else
            {
                SceneManager.LoadScene("PlayScene");
            }
        }
    }

    IEnumerator AnimateNewRecord()
    {
        Time.timeScale = 1.0f;
        yield return new WaitForSeconds(0.5f);
        newRecordText.SetActive(true);
        Utilities.UpdatePrefs("IsRecord", 0);
    }

	public void IsPhotoTaken(bool isPhotoTaken)
    {
        this.isPhotoTaken = isPhotoTaken;
    }

    public void OnClick()
    {
        settingsPanel.SetActive(menuToggle.isOn);
        configPanel.SetActive(menuToggle.isOn);
        framePanel.SetActive(!menuToggle.isOn);


        menuBackground.overrideSprite = (menuToggle.isOn) ? settingsOnSprite : settingsOffSprite;
        if (!isPhotoTaken){
            photoButton.SetActive(!menuToggle.isOn);

        }
        nextButton.SetActive(!menuToggle.isOn);
        backButton.SetActive(menuToggle.isOn);
    }
}
