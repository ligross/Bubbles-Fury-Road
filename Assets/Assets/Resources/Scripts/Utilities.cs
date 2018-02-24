using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Utilities {

	public static void UpdatePrefs(string prefName, int value)
	{
		int updatedValue = PlayerPrefs.GetInt (prefName, 0) + value;
		PlayerPrefs.SetInt (prefName, updatedValue);
		switch (updatedValue)
		{
		case 10:
			GooglePlayActions.UnlockAchievement (GooglePlayActions.beginnerAdsAchiev);
            PlayerPrefs.SetInt("MetallFrame", 1);
            break;
		case 20:
			GooglePlayActions.UnlockAchievement (GooglePlayActions.intermAdsAchiev);
            PlayerPrefs.SetInt("RainbowFrame", 1);
            break;
		case 30:
			GooglePlayActions.UnlockAchievement (GooglePlayActions.advancedAdsAchiev);
            PlayerPrefs.SetInt("GotFrame", 1);
            break;
        case 40:
            GooglePlayActions.UnlockAchievement(GooglePlayActions.superAdvancedAdsAchiev);
            PlayerPrefs.SetInt("GotFrame", 1);
            break;
        case 50:
            GooglePlayActions.UnlockAchievement(GooglePlayActions.mindBlowingAdsAchiev);
            PlayerPrefs.SetInt("GotFrame", 1);
            break;
        }
	}
}
