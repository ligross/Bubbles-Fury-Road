using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomFlying : MonoBehaviour {
    private float tChange = 0; // force new direction in the first Update
    private float randomX;
    private float randomY;

    private float moveSpeed = 2;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (Time.time >= tChange)
        {
            randomX = Random.Range(-2.0f, 2.0f); // with float parameters, a random float
            randomY = Random.Range(-2.0f, 2.0f); //  between -2.0 and 2.0 is returned
                                               // set a random interval between 0.5 and 1.5
            tChange = Time.time + Random.Range(0.5f, 1.5f);
        }
        transform.Translate(new Vector3(randomX, randomY, 0) * moveSpeed * Time.deltaTime);
    }
}
