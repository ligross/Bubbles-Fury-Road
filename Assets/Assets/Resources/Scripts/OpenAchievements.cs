using UnityEngine;

public class OpenAchievements : MonoBehaviour {
    public DoozyUI.UIElement notificationBackground;
    public DoozyUI.UIElement notificationBody;

    public void OnClick(){
        if (Social.localUser.authenticated)
        {
            Social.ShowAchievementsUI();
        }
		else
        {
            notificationBackground.Show(false);
            notificationBody.Show(false);
            Debug.Log("Cannot show achievements: not authenticated");
        }
	}
}
