using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class BadBubbleClick : MonoBehaviour {

	public GameObject blurObject;

	public void MouseDown() {
		#if UNITY_EDITOR
		if (Time.timeScale != 0 && !GameManager.Instance.starting && EventSystem.current.IsPointerOverGameObject() && GameManager.Instance.started) {
			Vector3 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
			pos.z = 0;
			//проверяем сколько клякс сгенерить
			if (Random.value > 0.5){
				float size = Random.Range(0.2f, 0.7f);
				GenerateBlur (pos, new Vector3(size, size, size));
				size = Random.Range(0.3f, 0.8f);
				StartCoroutine(GenerateBlurCoroutine(pos, new Vector3(size, size, size)));
			}
			else{
				GenerateBlur (pos, blurObject.transform.localScale);
			}

			GameManager.Instance.BurstBadBubble (gameObject);
			if (GameManager.Instance.visualBonus.IsActive ()) {
				gameObject.GetComponentInChildren<Animator> ().SetTrigger (GameManager.Instance.visualBonus.Name.ToString ());
				Debug.Log ("Acivated");
			}
		}
		#elif UNITY_ANDROID
		if (Time.timeScale != 0 && !GameManager.Instance.starting && EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId) && GameManager.Instance.started) {
			Vector3 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
			pos.z = 0;
			//проверяем сколько клякс сгенерить
			if (Random.value > 0.5){
                float size = Random.Range(0.2f, 0.7f);
				GenerateBlur (pos, new Vector3(size, size, size));
                size = Random.Range(0.3f, 0.8f);
                StartCoroutine(GenerateBlurCoroutine(pos, new Vector3(size, size, size)));
			}
			else{
				GenerateBlur (pos, blurObject.transform.localScale);
			}

			GameManager.Instance.BurstBadBubble (gameObject);
			if (GameManager.Instance.visualBonus.IsActive ()) {
				gameObject.GetComponentInChildren<Animator> ().SetTrigger (GameManager.Instance.visualBonus.Name.ToString ());
				Debug.Log ("Acivated");
			}
		}
		#endif
	}

	private void GenerateBlur(Vector3 pos, Vector3 size)
	{
		GameObject blur = Instantiate(blurObject, pos, Quaternion.identity) as GameObject;
		blur.transform.SetParent(GameObject.Find("Canvas").transform, false);
		blur.transform.SetAsFirstSibling();
        blur.transform.Rotate(new Vector3(0, 0, Random.Range(-180, 180)));
        blur.transform.localScale = size;
		blur.transform.position = pos;

    }

    IEnumerator GenerateBlurCoroutine(Vector3 pos, Vector3 size)
    {
        yield return new WaitForSeconds(0.5f);
        GenerateBlur(new Vector3(pos.x + Random.Range(-1f, 1f), pos.y, pos.z), size);
    }
}
