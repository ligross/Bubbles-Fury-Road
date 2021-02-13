using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ColorSelectButton : MonoBehaviour {
    public Toggle colorSelector;
    public RelaxBubbleColor color;

    public Sprite selectSprite;
    public Sprite unselectSprite;

	public void OnClick()
    {
        if (colorSelector.isOn)
        {
            RelaxManager.Instance.OnColorAdd(color);
            GetComponent<Image>().overrideSprite = selectSprite;
        }
        else
        {
            if (RelaxManager.Instance.selectedBubbleColors.Count > 1)
            {
                RelaxManager.Instance.OnColorRemove(color);
                GetComponent<Image>().overrideSprite = unselectSprite;
            }

        }

    }
}
