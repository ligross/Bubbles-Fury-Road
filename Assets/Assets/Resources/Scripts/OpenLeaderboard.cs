using UnityEngine;

public class OpenLeaderboard : MonoBehaviour {
    public DoozyUI.UIElement notificationBackground;
    public DoozyUI.UIElement notificationBody;

    public void OnClick(){
        if (Social.localUser.authenticated)
        {
            Social.ShowLeaderboardUI();
        }
        else
        {
            notificationBackground.Show(false);
            notificationBody.Show(false);
            Debug.Log("Cannot show leaderboard: not authenticated");
        }
	}
}
