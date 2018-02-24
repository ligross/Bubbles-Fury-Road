using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class LoadLevel : MonoBehaviour {
	public string levelName;

	public void OnClick() {
		SceneManager.LoadScene(levelName);
	}
}
