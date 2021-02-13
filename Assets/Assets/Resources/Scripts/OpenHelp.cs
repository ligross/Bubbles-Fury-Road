using UnityEngine;

public class OpenHelp : MonoBehaviour
{
	public DoozyUI.UIElement helpBackground;
	public DoozyUI.UIElement helpBody;

    public bool IsOpened()
    {
        return helpBody.GetComponent<Canvas>().enabled;
    }

	public void OnClick()
	{
        //this.GetComponent<GameObject>().SetActive(true);
        helpBackground.Show(false);
        helpBody.Show(false);
        if (!PlayerPrefs.HasKey("HelpSeen"))
        {
            GooglePlayActions.UnlockAchievement(GooglePlayActions.goodPupilAchiev);
            PlayerPrefs.SetInt("HelpSeen", 1);
        }
    }
}
