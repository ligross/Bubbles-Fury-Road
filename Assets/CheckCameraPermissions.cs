using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Android;
using UnityEngine.SceneManagement;

public class CheckCameraPermissions : MonoBehaviour
{
    public void OnClick()
    {
        if (Permission.HasUserAuthorizedPermission(Permission.Camera))
        {
            SceneManager.LoadScene("PhotoScene");
        }
        else
        {
            AndroidRuntimePermissions.Permission result = AndroidRuntimePermissions.RequestPermission("android.permission.CAMERA");
            if (result == AndroidRuntimePermissions.Permission.Granted)
                SceneManager.LoadScene("PhotoScene");
        }
    }
}
