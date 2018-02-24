using UnityEngine;
using System.Collections;
using GooglePlayGames;
using GooglePlayGames.BasicApi;
using UnityEngine.SocialPlatforms;
using UnityEngine.UI;


public class GooglePlayManager : MonoBehaviour 
{
	private static GooglePlayManager instance = null;

	void Awake()
	{
		if (instance == null) // check to see if the instance has a refrence
		{
			instance = this; // if not, give it a refrence to this class...
			DontDestroyOnLoad(this.gameObject); // and make this object persistant as we load new scenes
		} 
		else // if we already have a refrence then remove the extra manager from the scene
		{
			Destroy(this.gameObject);
		}
	}

	void Start(){
        // recommended for debugging:
        PlayGamesClientConfiguration config = new PlayGamesClientConfiguration.Builder().Build();
        PlayGamesPlatform.InitializeInstance(config);
        PlayGamesPlatform.DebugLogEnabled = true;
        // Activate the Google Play Games platform
        PlayGamesPlatform.Activate();

        if (!Social.localUser.authenticated)
        {
            Social.localUser.Authenticate((bool success) =>
            {
                if (success)
                {
                    Debug.Log("Successfully authenticated!");
                }
                else
                {
                    Debug.Log("Error! Not authenticated!");
                }
            });
        }
        else
        {
            Debug.Log("\t already authenticated as: " + Social.localUser.userName);
        }
    }

	void OnApplicationQuit()
	{
		PlayGamesPlatform.Instance.SignOut();
	}
}
