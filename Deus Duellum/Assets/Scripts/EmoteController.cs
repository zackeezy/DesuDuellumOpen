using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EmoteController : MonoBehaviour {

    public int character;

    private string HelloEmote;
    private string WowEmote;
    private string TauntEmote;

    private float voiceLineLength;
    private AudioClip HelloVoice;
    private AudioClip WowVoice;
    private AudioClip TauntVoice;

    private GameObject emotePanel;
    private Text emoteText;

    // Use this for initialization
    void Start () {
        //temporary...
        voiceLineLength = 2;
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void SetCharacter(int character)
    {
        //function is called in BoardManager's setPrefs()
        if (character == 0)
        {
            //athena's emotes
            HelloEmote = "Athena's Hello";
            WowEmote = "Athena's Wow";
            TauntEmote = "Athena's Taunt";
            //set the voicelines

        }
        else if (character == 1)
        {
            //ra's emotes
            HelloEmote = "Ra's Hello";
            WowEmote = "Ra's Wow";
            TauntEmote = "Ra's Taunt";
            //set the voicelines

        }
        else if (character == 2)
        {
            //thor's emotes
            HelloEmote = "Thor's Hello";
            WowEmote = "Thor's Wow";
            TauntEmote = "Thor's Taunt";
            //set the voicelines

        }
    }

    public void EmoteClicked(int emote)
    {
        //deactivate emote buttons
        GameObject emoteButtons = transform.GetChild(3).gameObject;
        emoteButtons.SetActive(false);

        //get the emote panel
        emotePanel = transform.GetChild(4).gameObject;
        emoteText = emotePanel.transform.GetChild(0).GetComponent<Text>();

        PlayEmote(emote);
        StartCoroutine(DeactivateLocalEmotePanel());
    }

    public void OtherEmote(int emote)
    {
        //get the emote panel
        emotePanel = transform.GetChild(3).gameObject;
        Text emoteText = emotePanel.transform.GetChild(0).GetComponent<Text>();

        PlayEmote(emote);
        StartCoroutine(DeactivateOtherEmotePanel());
    }

    public void PlayEmote(int emote)
    {
        //change the text of emote panel and which voiceline to play
        AudioClip emoteClip = new AudioClip();
        if (emote == 0)
        {
            //hello
            emoteText.text = HelloEmote;
            emoteClip = HelloVoice;
        }
        else if (emote == 1)
        {
            //wow
            emoteText.text = WowEmote;
            emoteClip = WowVoice;
        }
        else if (emote == 2)
        {
            //taunt
            emoteText.text = TauntEmote;
            emoteClip = TauntVoice;
        }
        //voiceLineLength = emoteClip.length;

        //set the clip the emoteSource uses
        //GameObject Audio = GameObject.FindGameObjectWithTag("Audio");
        //AudioSource emoteSource = Audio.GetComponent<MusicInfo>().emotesSource.GetComponent<AudioSource>();
        //emoteSource.clip = emoteClip;
        ////play the clip
        //emoteSource.Play();

        //activate the emote panel        
        emotePanel.SetActive(true);
    }

    IEnumerator DeactivateLocalEmotePanel()
    {
        //TODO: ANIMATE IT WITH LEANTWEEN INSTEAD

        //deactivate emotebutton
        Button emoteButton = transform.GetChild(2).GetComponent<Button>();
        emoteButton.interactable = false;

        yield return new WaitForSeconds(voiceLineLength);
        emotePanel.SetActive(false);
        //reactivate emote button
        emoteButton.interactable = true;
    }

    IEnumerator DeactivateOtherEmotePanel()
    {
        //TODO: ANIMATE IT WITH LEANTWEEN INSTEAD

        yield return new WaitForSeconds(voiceLineLength);
        emotePanel.SetActive(false);
    }
}
