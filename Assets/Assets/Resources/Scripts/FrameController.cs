using UnityEngine;
using UnityEngine.UI;

public class FrameController : MonoBehaviour {
	
	public GameObject kingFrame;
	public GameObject pinkFrame;
	public GameObject metallFrame;
    public GameObject carpetFrame;
    public GameObject dragonFrame;
    public GameObject goldenFrame;
    public GameObject gotFrame;
    public GameObject heroFrame;
    public GameObject rainbowFrame;

    public Image framePlace;

	void Start()
	{
		if (PlayerPrefs.GetInt ("KingFrame") == 1) {
			GameObject frame = Instantiate (kingFrame);
			frame.transform.SetParent (this.transform, false);
            frame.GetComponent<FramePicker>().framePlace = framePlace;

        }
		if (PlayerPrefs.GetInt ("PinkFrame") == 1) {
			GameObject frame = Instantiate (pinkFrame);
			frame.transform.SetParent (this.transform, false);
            frame.GetComponent<FramePicker>().framePlace = framePlace;
        }
		if (PlayerPrefs.GetInt ("MetallFrame") == 1) {
			GameObject frame = Instantiate (metallFrame);
			frame.transform.SetParent (this.transform, false);
            frame.GetComponent<FramePicker>().framePlace = framePlace;
        }
        if (PlayerPrefs.GetInt("CarpetFrame") == 1)
        {
            GameObject frame = Instantiate(carpetFrame);
            frame.transform.SetParent(this.transform, false);
            frame.GetComponent<FramePicker>().framePlace = framePlace;
        }
        if (PlayerPrefs.GetInt("DragonFrame") == 1)
        {
            GameObject frame = Instantiate(dragonFrame);
            frame.transform.SetParent(this.transform, false);
            frame.GetComponent<FramePicker>().framePlace = framePlace;
        }
        if (PlayerPrefs.GetInt("GoldenFrame") == 1)
        {
            GameObject frame = Instantiate(goldenFrame);
            frame.transform.SetParent(this.transform, false);
            frame.GetComponent<FramePicker>().framePlace = framePlace;
        }
        if (PlayerPrefs.GetInt("GotFrame") == 1)
        {
            GameObject frame = Instantiate(gotFrame);
            frame.transform.SetParent(this.transform, false);
            frame.GetComponent<FramePicker>().framePlace = framePlace;
        }
        if (PlayerPrefs.GetInt("HeroFrame") == 1)
        {
            GameObject frame = Instantiate(heroFrame);
            frame.transform.SetParent(this.transform, false);
            frame.GetComponent<FramePicker>().framePlace = framePlace;
        }
        if (PlayerPrefs.GetInt("RainbowFrame") == 1)
        {
            GameObject frame = Instantiate(rainbowFrame);
            frame.transform.SetParent(this.transform, false);
            frame.GetComponent<FramePicker>().framePlace = framePlace;
        }
    }
}
