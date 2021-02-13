using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnAnimationFinished : MonoBehaviour
{
    public void SetOff()
    {
        this.gameObject.SetActive(false);
    }

    public void SetTrigger()
    {
        gameObject.GetComponent<Animator>().SetTrigger("Current");
        //gameObject.GetComponent<Animator>().ResetTrigger("MoveFromLeft");
    }
}
