using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class RelaxBubbleClick : MonoBehaviour {
    public RelaxBubbleColor color;

    Vector2 startPosition;

    public void PointerDown()
    {

        // Input.GetTouch (0).fingerId  
#if UNITY_EDITOR
        if (Time.timeScale > 0 && EventSystem.current.IsPointerOverGameObject() && RelaxManager.Instance.started)
        {
            //startPosition = Input.GetTouch(0).position;
        }
#elif UNITY_ANDROID
        if (Time.timeScale > 0 && EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId) && RelaxManager.Instance.started) {
            startPosition = Input.GetTouch(0).position;
        }
#endif
    }

    public void PointerUp()
    {

        // Input.GetTouch (0).fingerId  
#if UNITY_EDITOR
        //if (Time.timeScale > 0 && EventSystem.current.IsPointerOverGameObject() && RelaxManager.Instance.started)
        //{   if (endPosition.magnitude < 0.01)
        //    {
                //RelaxManager.Instance.BurstRelaxBubble(gameObject);
        //    }
        //}
#elif UNITY_ANDROID
        if (Time.timeScale > 0 && EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId) && RelaxManager.Instance.started) {
        Vector2 endPosition = Input.GetTouch(0).position - startPosition;
            if (endPosition.magnitude < 0.01)
            {
                RelaxManager.Instance.BurstRelaxBubble(gameObject);
            }
        }
#endif
    }
}

