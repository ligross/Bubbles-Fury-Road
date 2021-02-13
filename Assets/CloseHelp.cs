using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CloseHelp : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void onHelpClose()
    {
        this.GetComponentInChildren<Text>().text = "OUCH!";
        this.GetComponent<Animator>().SetTrigger("Close");

    }
}
