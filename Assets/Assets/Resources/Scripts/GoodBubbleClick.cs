using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class GoodBubbleClick : MonoBehaviour {

	public void MouseDown() {
		// Input.GetTouch (0).fingerId
		if (Time.timeScale != 0 && EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId)) {
			GameManager.Instance.BurstGoodBubble (gameObject);
			if (GameManager.Instance.visualBonus.IsActive())
				gameObject.GetComponentInChildren<Animator> ().SetTrigger (GameManager.Instance.visualBonus.Name.ToString());
		}
	}
}
