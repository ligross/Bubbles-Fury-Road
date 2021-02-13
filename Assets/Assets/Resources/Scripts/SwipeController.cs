using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DigitalRubyShared;

public class SwipeController : MonoBehaviour
{

    public PhotoPickerController photoPickerController;
    public SwipeGestureRecognizerEndMode SwipeMode = SwipeGestureRecognizerEndMode.EndImmediately;

    private SwipeGestureRecognizer swipe;

    private void Start()
    {
        swipe = new SwipeGestureRecognizer();
        swipe.StateUpdated += Swipe_Updated;
        swipe.DirectionThreshold = 0;
        swipe.MinimumNumberOfTouchesToTrack = swipe.MaximumNumberOfTouchesToTrack = 1;
        FingersScript.Instance.AddGesture(swipe);
    }

    private void Update()
    {
        swipe.MinimumNumberOfTouchesToTrack = swipe.MaximumNumberOfTouchesToTrack = 1;
        swipe.EndMode = SwipeMode;
    }

    private void Swipe_Updated(DigitalRubyShared.GestureRecognizer gesture)
    {
        //Debug.LogFormat("Swipe state: {0}", gesture.State);

        SwipeGestureRecognizer swipe = gesture as SwipeGestureRecognizer;
        if (swipe.State == GestureRecognizerState.Ended)
        {
            if (swipe.EndDirection.Equals(SwipeGestureRecognizerDirection.Right))
            {
                photoPickerController.SwipeRight();
            }
            else if (swipe.EndDirection.Equals(SwipeGestureRecognizerDirection.Left))
            {
                photoPickerController.SwipeLeft();
            }
        }
    }
}
