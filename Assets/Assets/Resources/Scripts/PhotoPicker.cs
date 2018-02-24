using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhotoPicker : MonoBehaviour {

	public string name;
	private PhotoPickerController controller;

	void Start(){
		controller = GameObject.Find ("PhotoPanel").GetComponent<PhotoPickerController>();
	}

	public void PickPhoto()
	{
		controller.PickPhoto (this.name);
	}
}
