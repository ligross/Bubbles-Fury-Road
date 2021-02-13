using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RelaxMenuController : MonoBehaviour
{
    public GameObject backButton;
    public GameObject nextButton;
    public GameObject backText;
    public GameObject menuText;
    public GameObject newGameButton;

    public GameObject colorSelectPanel;
    public GameObject sizeSelectPanel;
    public GameObject settingsPanel;

    private bool _isOpened;

    public bool IsOpened()
    {
        return _isOpened;
    }

    public void onClick()
    {
        _isOpened = !_isOpened;

        menuText.GetComponent<Text>().text = (_isOpened) ? "GO TO GAME" : "GO TO MENU";
        backText.SetActive(_isOpened);
        backButton.GetComponent<Image>().enabled = _isOpened;
        //nextButton.SetActive(!_isOpened);
        newGameButton.SetActive(!_isOpened);
        colorSelectPanel.SetActive(!_isOpened);
        sizeSelectPanel.SetActive(!_isOpened);
        settingsPanel.SetActive(_isOpened);

    }
}
