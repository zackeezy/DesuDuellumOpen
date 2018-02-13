using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

//connects sliders to audio sources
public class VolumeConnector : MonoBehaviour {

	public Slider musicSlider;
	public Slider effectsSlider;
	public Slider emotesSlider;
    public Toggle muteToggle;

    // Use this for initialization
    void Start () {
		//set the sliders to match the volumes from the player preferences
		musicSlider.value = PlayerPrefs.GetFloat("music", 1);
		effectsSlider.value = PlayerPrefs.GetFloat("effects", 1);
		emotesSlider.value = PlayerPrefs.GetFloat("emotes", 1);

        //check to see if sound is muted
        muteToggle.isOn = (PlayerPrefs.GetInt("muted", 0) == 1);
	}
	
	// Update is called once per frame
	//update the volumes in the player preferences with the slider values
	void Update () {
		PlayerPrefs.SetFloat("music", musicSlider.value);
        PlayerPrefs.SetFloat("effects", effectsSlider.value);
        PlayerPrefs.SetFloat("emotes", emotesSlider.value);
        if (muteToggle.isOn)
        {
            PlayerPrefs.SetInt("muted", 1);
        }
        else
        {
            PlayerPrefs.SetInt("muted", 0);
        }
    }
}
