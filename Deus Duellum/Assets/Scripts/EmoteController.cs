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
        //temp
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
        GameObject emoteButtons = transform.GetChild(4).gameObject;
        emoteButtons.SetActive(false);

        //get the emote panel
        emotePanel = transform.GetChild(5).gameObject;
        emoteText = emotePanel.transform.GetChild(0).GetComponent<Text>();

        PlayEmoteAudio(emote);
        StartCoroutine(AnimateLocalEmotePanel());
    }

    public void OtherEmote(int emote)
    {
        //get the emote panel
        emotePanel = transform.GetChild(3).gameObject;
        Text emoteText = emotePanel.transform.GetChild(0).GetComponent<Text>();

        PlayEmoteAudio(emote);
        StartCoroutine(AnimateLocalEmotePanel());
    }

    public void PlayEmoteAudio(int emote)
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
    }

    IEnumerator AnimateLocalEmotePanel()
    {
        //activate the emote panel
        emotePanel.SetActive(true);

        //deactivate emotebutton
        Button emoteButton = transform.GetChild(3).GetComponent<Button>();
        emoteButton.interactable = false;

        //animate the emote button
        Vector3 oldScale = emotePanel.transform.localScale;
        LeanTween.scale(emotePanel, emotePanel.transform.localScale * 1.2f, 0.1f);
        LeanTween.scale(emotePanel, oldScale, 0.1f).setDelay(.1f);

        yield return new WaitForSeconds(voiceLineLength);
        emotePanel.SetActive(false);

        //reactivate emote button
        emoteButton.interactable = true;
    }

    IEnumerator AnimateOtherEmotePanel()
    {
        Vector3 oldScale = emotePanel.transform.localScale;
        LeanTween.scale(emotePanel, emotePanel.transform.localScale * 1.2f, 0.15f);
        LeanTween.scale(emotePanel, oldScale, 0.15f).setDelay(.15f);

        yield return new WaitForSeconds(voiceLineLength);
        emotePanel.SetActive(false);
    }
}
