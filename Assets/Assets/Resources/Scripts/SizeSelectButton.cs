using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SizeSelectButton : MonoBehaviour
{
    public Toggle sizeSelector;
    public RelaxBubbleSize size;

    public Sprite selectSprite;
    public Sprite unselectSprite;

    public void OnClick()
    {
        if (sizeSelector.isOn)
        {
            RelaxManager.Instance.OnSizeChanged(size);
            GetComponent<Image>().overrideSprite = selectSprite;
        }
        else
        {
            GetComponent<Image>().overrideSprite = unselectSprite;
        }

    }
}
