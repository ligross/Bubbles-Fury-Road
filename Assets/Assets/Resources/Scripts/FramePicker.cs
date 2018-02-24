using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FramePicker : MonoBehaviour {

	public Sprite frameToSet;
	public Image framePlace;
    public Material frameMaterial;

	public void PickImage(){
		framePlace.sprite = frameToSet;
        framePlace.material = frameMaterial;
	}
}
