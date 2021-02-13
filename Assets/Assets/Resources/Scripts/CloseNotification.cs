using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloseNotification : MonoBehaviour {
    public DoozyUI.UIElement notificationBackground;
    public DoozyUI.UIElement notificationBody;

    public bool IsOpened()
    {
        return notificationBody.GetComponent<Canvas>().isActiveAndEnabled;
    }

    public void Close()
    {
        notificationBackground.Hide(false);
        notificationBody.Hide(false);

        if (GameManager.Instance.isViewingHelp)
        {
            Debug.Log("Closed!");
            GameManager.Instance.isViewingHelp = false;
            StartCoroutine(GameManager.Instance.StartGame());
        }
    }
}
