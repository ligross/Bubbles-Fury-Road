using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class ScoreAdder : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        gameObject.transform.Translate(new Vector3(0, 5, 0) * Time.deltaTime);
        GetComponent<Text>().CrossFadeAlpha(0, 0.7f, false);
    }

    public void AddScore(int score)
    {
       GetComponent<Text>().text = score.ToString();
    }
}
