using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuController : MonoBehaviour {
	public GameObject photoButton;
	public GameObject backButton;
	public GameObject relaxButton;
	public GameObject nextButton;
	public GameObject backText;
	public GameObject backButtonIngame;

	private bool _isOpened;

    public bool IsOpened()
    {
        return _isOpened;
    }

	public void onClick(){
		_isOpened = !_isOpened;

		if (GameManager.Instance.started && !GameManager.Instance.finished) {
            if (relaxButton != null)
            {
                relaxButton.SetActive(false);
            }
			photoButton.SetActive (false);
			backButton.GetComponent<Image>().enabled = false;
			nextButton.SetActive (false);
			backText.SetActive (false);
			backButtonIngame.SetActive (true);

		} else if (GameManager.Instance.finished) {
            if (relaxButton != null)
            {
                relaxButton.SetActive(false);
            }
			nextButton.SetActive (true);
			backButton.GetComponent<Image>().enabled = false;
			backText.SetActive (false);
            backButtonIngame.SetActive(false);
		}
		else {
            if (relaxButton != null){
                relaxButton.SetActive(!_isOpened);
            }
			backText.SetActive (_isOpened);
			backButton.GetComponent<Image>().enabled = _isOpened;
			backText.SetActive (_isOpened);
			photoButton.SetActive (false);
			nextButton.SetActive (false);
			backButtonIngame.SetActive (false);
		}
	}
}
