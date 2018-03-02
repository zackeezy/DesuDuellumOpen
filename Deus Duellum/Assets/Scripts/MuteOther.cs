using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MuteOther : MonoBehaviour {

    public Sprite mutedImg;
    public Sprite unMutedImg;

    private bool muted = false;
    private Image buttonImg;

	// Use this for initialization
	void Start () {
        buttonImg = transform.GetComponent<Image>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void toggleMute()
    {
        muted = !muted;
        if (muted)
        {
            buttonImg.sprite = mutedImg;
            //tell the boardmanager to stop accepting emotes from AI or net

        }
        else
        {
            buttonImg.sprite = unMutedImg;
            //tell the boardmanager to start accepting emotes from AI or net

        }
    }
}
