using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HelpSwiper : MonoBehaviour
{
	// Start is called before the first frame update
	public GameObject[] slides;
    public GameObject[] dots;

    public Sprite dotSelectedImage;
    public Sprite dotUnselectedImage;

    private int activeSlideNumber = 0;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void NextSlide()
	{
        if (activeSlideNumber < slides.Length - 1)
		{
            Debug.Log("Next");
			int nextSlideNumber = activeSlideNumber + 1;
            slides[activeSlideNumber].GetComponent<Animator>().SetTrigger("MoveToLeft");
            dots[activeSlideNumber].GetComponent<Image>().overrideSprite = dotUnselectedImage;
            activeSlideNumber = nextSlideNumber;
            dots[activeSlideNumber].GetComponent<Image>().overrideSprite = dotSelectedImage;
            Debug.Log(activeSlideNumber);
        }
	}

    public void PreviousSlide()
    {
        if (activeSlideNumber > 0)
        {
            Debug.Log("Prev");
            int nextSlideNumber = activeSlideNumber - 1;
            slides[nextSlideNumber].SetActive(true);
            slides[nextSlideNumber].GetComponent<Animator>().SetTrigger("MoveFromLeft");
            dots[activeSlideNumber].GetComponent<Image>().overrideSprite = dotUnselectedImage;
            activeSlideNumber = nextSlideNumber;
            dots[activeSlideNumber].GetComponent<Image>().overrideSprite = dotSelectedImage;
            Debug.Log(activeSlideNumber);
        }
    }

    public void MoveToSlide(int slideNumber)
    {
        if (this.activeSlideNumber > slideNumber) {
            for (int i = (activeSlideNumber - 1); i >= slideNumber; i--)
            {
                PreviousSlide();
            }
        }
        else if (this.activeSlideNumber < slideNumber)
        {
            for (int i = activeSlideNumber; i < slideNumber; i++)
            {
                NextSlide();
            }
        }
    }
}
