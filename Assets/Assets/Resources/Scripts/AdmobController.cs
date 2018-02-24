using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleMobileAds;
using GoogleMobileAds.Api;
using System;
using UnityEngine.UI;

public class AdmobController : MonoBehaviour {

    private const string _appId = "ca-app-pub-1665385743848421~7113529191";

	private const string _bonusAds = "ca-app-pub-1665385743848421/1566523194";

	private bool rewardBasedEventHandlersSet = false;
	public bool isAdsAvailable = true;

	RewardBasedVideoAd rewardBasedVideo;

	public GameObject errorText;
	public Button pauseButton;

	public void RequestRewardBasedVideo()
	{
		pauseButton.interactable = false;
		GetComponent<Button> ().interactable = false;

		rewardBasedVideo = RewardBasedVideoAd.Instance;
		if (!rewardBasedEventHandlersSet)
		{
			// Ad event fired when the rewarded video ad
			// has been received.
			rewardBasedVideo.OnAdLoaded += HandleRewardBasedVideoLoaded;
			// has failed to load.
			rewardBasedVideo.OnAdFailedToLoad += HandleRewardBasedVideoFailedToLoad;

			// has rewarded the user.
			rewardBasedVideo.OnAdRewarded += HandleRewardBasedVideoRewarded;

			rewardBasedEventHandlersSet = true;
		}
		AdRequest request = new AdRequest.Builder().AddTestDevice(AdRequest.TestDeviceSimulator).AddTestDevice("7674860455C6D237").AddTestDevice("639F9DCDBC7EFF08").Build();
		rewardBasedVideo.LoadAd(request, _bonusAds);
	}

	private void HandleRewardBasedVideoRewarded(object sender, Reward args)
	{
        pauseButton.interactable = true;
        Debug.Log("User rewarded with bonus");   
		Utilities.UpdatePrefs ("WatchAdsCounter", 1);
		GameManager.Instance._gridManager.BonusSpawnPerc = 1;
		rewardBasedEventHandlersSet = false;
	}


	private void HandleRewardBasedVideoFailedToLoad(object sender, AdFailedToLoadEventArgs args)
	{
		pauseButton.interactable = true;
		Debug.Log(args.Message); 
		errorText.SetActive (true);

		switch (args.Message) {
		case "No fill":
			errorText.GetComponent<Text> ().text = (PlayerPrefs.GetString("language", "EN").Equals("EN"))? "No ads available today :(" : "На сегодня рекламки больше нет :(";
			isAdsAvailable = false;
			break;
		default:
			errorText.GetComponent<Text> ().text = (PlayerPrefs.GetString("language", "EN").Equals("EN")) ? "I'm broken!" : "Я сломался!";
			break;
		}

	}

	private void HandleRewardBasedVideoLoaded(object sender, EventArgs args)
	{
		rewardBasedVideo.Show();
		pauseButton.interactable = true;
	}
}
