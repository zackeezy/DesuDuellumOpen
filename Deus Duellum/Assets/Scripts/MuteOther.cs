using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MuteOther : MonoBehaviour {

    public Sprite mutedImg;
    public Sprite unMutedImg;

    private bool muted = false;
    private Image buttonImg;

    public EmoteController player2Emotes;

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
            player2Emotes.emotesMuted = true;
        }
        else
        {
            buttonImg.sprite = unMutedImg;
            //tell the boardmanager to start accepting emotes from AI or net
            player2Emotes.emotesMuted = false;
        }
    }
}
