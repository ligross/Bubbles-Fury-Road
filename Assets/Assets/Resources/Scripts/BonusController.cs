using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class BonusController : MonoBehaviour {
	public Text bonusName;
	public Image timeImage;

	public GameObject freeIndicator;
	public GameObject visualIndicator;

	private Bonus _bonus;
	private Bonus _freeBonus;

	
	// Update is called once per frame
	void Update () {
		if (GameManager.Instance.bonus.IsActive ()) {
			bonusName.text = string.Format ("{0} +{1}%", GameManager.Instance.bonus.Name, GameManager.Instance.bonus.Multiplier * 100);
			float amount = Math.Abs (GameManager.Instance.bonus.TimeTillEnd() / 10f);
			if (GameManager.Instance.bonus.TimeTillEnd() > 0)
				timeImage.fillAmount = amount;
			else 
				timeImage.fillAmount = 0;
		} else {
			bonusName.text = "";

		}
		if (GameManager.Instance.freeBonus.IsActive ()) {
			freeIndicator.SetActive (true);
		} else {
			freeIndicator.SetActive (false);
		}

		if (GameManager.Instance.visualBonus.IsActive ()) {
			visualIndicator.SetActive (true);
		} else {
			visualIndicator.SetActive (false);
		}
	}
}
