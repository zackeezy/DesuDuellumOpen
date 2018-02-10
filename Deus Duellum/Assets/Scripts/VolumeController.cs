using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VolumeController : MonoBehaviour {

    public Slider volume;
    public AudioSource mySound;
    private bool toggle = false;

	// Update is called once per frame
	void Update () {
        mySound.volume = volume.value;
	}

    public void ToggleSounds()
    {
        toggle = !toggle;

        AudioListener.pause = toggle;
    }
}
