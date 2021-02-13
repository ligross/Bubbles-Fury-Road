using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class BonusBubbleClick : MonoBehaviour {

	public void MouseDown() {
		#if UNITY_EDITOR
		if (Time.timeScale != 0 && EventSystem.current.IsPointerOverGameObject() && GameManager.Instance.started) {
			Destroy(gameObject.GetComponent<EventTrigger>());
			GameManager.Instance.BurstBonusBubble (gameObject);
			if (GameManager.Instance.visualBonus.IsActive())
				gameObject.GetComponentInChildren<Animator> ().SetTrigger (GameManager.Instance.visualBonus.Name.ToString());
		}

		// Input.GetTouch (0).fingerId
        #elif UNITY_ANDROID
		if (Time.timeScale != 0 && EventSystem.current.IsPointerOverGameObject(Input.GetTouch (0).fingerId) && GameManager.Instance.started) {
            Destroy(gameObject.GetComponent<EventTrigger>());
            GameManager.Instance.BurstBonusBubble (gameObject);
			if (GameManager.Instance.visualBonus.IsActive())
				gameObject.GetComponentInChildren<Animator> ().SetTrigger (GameManager.Instance.visualBonus.Name.ToString());
		}
		#endif


	}
}
