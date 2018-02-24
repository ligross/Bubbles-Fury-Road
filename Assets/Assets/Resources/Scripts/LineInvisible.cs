using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineInvisible : MonoBehaviour {

	void OnBecameInvisible (){
		if (GameManager.Instance != null) {
			GameManager.Instance.OnLineDeleted ();
		}
	}
}
