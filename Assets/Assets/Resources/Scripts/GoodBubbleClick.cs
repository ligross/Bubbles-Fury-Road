using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class GoodBubbleClick : MonoBehaviour {

	public void MouseDown() {
		// Input.GetTouch (0).fingerId	
		#if UNITY_EDITOR
		if (Time.timeScale != 0 && EventSystem.current.IsPointerOverGameObject() && GameManager.Instance.started) {

			GameManager.Instance.BurstGoodBubble (gameObject);
			if (GameManager.Instance.visualBonus.IsActive())
				gameObject.GetComponentInChildren<Animator> ().SetTrigger (GameManager.Instance.visualBonus.Name.ToString());
		}
        #elif UNITY_ANDROID
		if (Time.timeScale != 0 && EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId) && GameManager.Instance.started) {
			GameManager.Instance.BurstGoodBubble (gameObject);
			if (GameManager.Instance.visualBonus.IsActive())
				gameObject.GetComponentInChildren<Animator> ().SetTrigger (GameManager.Instance.visualBonus.Name.ToString());
		}
		#endif
		}

}

