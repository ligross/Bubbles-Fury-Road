using UnityEngine;
using UnityEngine.Advertisements;
using UnityEngine.UI;

public class AdmobController : MonoBehaviour {

    private const string _appId = "ca-app-pub-1665385743848421~7113529191";

	private const string _bonusAds = "ca-app-pub-1665385743848421/1566523194";

	public bool isAdsAvailable = true;


	public GameObject errorText;
	public Button pauseButton;

    public void ShowRewardedAd()
    {
        if (Advertisement.IsReady("rewardedVideo"))
        {
            var options = new ShowOptions { resultCallback = HandleShowResult };
            Advertisement.Show("rewardedVideo", options);
        }
        else
        {
            pauseButton.interactable = true;
            errorText.SetActive(true);
            errorText.GetComponent<Text>().text = "Oooops! Ads is not available right now, please try again later";
        }
    }

    private void HandleShowResult(ShowResult result)
    {
        pauseButton.interactable = false;
        GetComponent<Button> ().interactable = false;
        Text text = errorText.GetComponent<Text>();
        text.color = new Color(text.color.r, text.color.g, text.color.b, 0.8f);

        switch (result)
        {
            case ShowResult.Finished:
                Debug.Log("The ad was successfully shown.");
                pauseButton.interactable = true;
                Utilities.UpdatePrefs ("WatchAdsCounter", 1);
                GameManager.Instance._gridManager.BonusSpawnPerc = 1;
                errorText.GetComponent<Text>().text = "Bonus bubbles are spawned on the trace!";
                break;
            case ShowResult.Skipped:
                Debug.Log("The ad was skipped before reaching the end.");
                pauseButton.interactable = true;
                errorText.SetActive (true);
                errorText.GetComponent<Text>().text = "The ad was skipped before reaching the end";
                break;
            case ShowResult.Failed:
                Debug.LogError("The ad failed to be shown.");
                pauseButton.interactable = true;
                errorText.SetActive(true);
                errorText.GetComponent<Text>().text = "The ad failed to be shown";
                break;
        }
    }
}
