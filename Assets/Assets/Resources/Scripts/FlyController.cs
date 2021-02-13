using UnityEngine;
using UnityEngine.EventSystems;

public class FlyController : MonoBehaviour {

	private int _touchesCount;

    public void MouseDown()
    {
		GameManager.Instance.SetCarma (-5);

		#if UNITY_EDITOR
		if (Time.timeScale != 0 && EventSystem.current.IsPointerOverGameObject () && GameManager.Instance.started)
		{
			if (!GetComponent<RandomFlying> ().enabled) GameManager.Instance.FrightenFly(gameObject);
			else this.GetComponent<Animator> ().SetTrigger ("Clicked");
		}
        #elif UNITY_ANDROID
		if (Time.timeScale != 0 && EventSystem.current.IsPointerOverGameObject (Input.GetTouch (0).fingerId) && GameManager.Instance.started)
        {
			if (!GetComponent<RandomFlying> ().enabled) GameManager.Instance.FrightenFly(gameObject);
			else this.GetComponent<Animator> ().SetTrigger ("Clicked");
        }
		#endif

		_touchesCount += 1;

		//разблокировать мухобойку
		if (_touchesCount >= 5) {
			GooglePlayActions.UnlockAchievement (GooglePlayActions.flySwatAchiev);
		}
    }

    void OnBecameInvisible()
    {
        Destroy(gameObject);
    }
}
