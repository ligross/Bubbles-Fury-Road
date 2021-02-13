using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhotoPicker : MonoBehaviour {

	public int ind;
	private PhotoPickerController controller;

	void Start(){
		controller = GameObject.Find ("Canvas").GetComponent<PhotoPickerController>();
	}

	public void PickPhoto()
	{
		controller.PickPhoto (ind);
	}
}
