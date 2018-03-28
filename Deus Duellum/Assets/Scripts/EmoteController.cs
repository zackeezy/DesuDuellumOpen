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
    public AudioClip[] HelloVoices;
    public AudioClip[] WowVoices;
    public AudioClip[] TauntVoices;

    private GameObject emoteButtons;
    private GameObject emotePanel;
    private Text emoteText;

    AudioSource emoteSource;

    // Use this for initialization
    void Start () {
        //temporary
        voiceLineLength = 3;

        emoteButtons = transform.GetChild(4).gameObject;
        emotePanel = transform.GetChild(5).gameObject;

        //set the clip the emoteSource uses
        //WILL NOT WORK IF DO NOT START AT MAIN MENU
        GameObject Audio = GameObject.FindGameObjectWithTag("Audio");
        emoteSource = Audio.GetComponent<MusicInfo>().emotesSource.GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update () {
		
	}

    public void OpenEmoteButtons()
    {
        if (emoteButtons.activeSelf)
        {
            CloseEmoteButtons();
        }
        else
        {
            emoteButtons.SetActive(true);
        }
    }

    public void CloseEmoteButtons()
    {
        emoteButtons.SetActive(false);  
    }

    public void SetCharacter(int givenCharacter)
    {
        //function is called in BoardManager's setPrefs()

        character = givenCharacter;
        if (character == 0)
        {
            //athena's emotes
            HelloEmote = "Yamas! Good health to all.";
            WowEmote = "Even the goddess of strategy \ncouldn't see that coming.";
            TauntEmote = "You remind me a lot of Koalemos. \nHe's the god of stupidity.\n Not that you would know";
        }
        else if (character == 1)
        {
            //ra's emotes
            HelloEmote = "Good morrow from the Sun.";
            WowEmote = "You shine brighter than the sun!";
            TauntEmote = "Even Anubis couldn't tip \nthe scales in your favor.";
        }
        else if (character == 2)
        {
            //thor's emotes
            HelloEmote = "Good health, my friend.";
            WowEmote = "By Odin's beard!";
            //TauntEmote = "You can't even spell Meal-near!";
            TauntEmote = "You can't even spell Mjolnir!";
        }
    }

    public void EmoteClicked(int emote)
    {
        //deactivate emote buttons
        emoteButtons.SetActive(false);
        ToggleOpen panelManager = GameObject.FindGameObjectWithTag("panelManager").GetComponent<ToggleOpen>();
        panelManager.SelectedPanel = null;

        //get the emote text
        emoteText = emotePanel.transform.GetChild(0).GetComponent<Text>();

        PlayEmoteAudio(emote);
        StartCoroutine(AnimateLocalEmotePanel());
    }

    public void OtherEmote(int character)
    {
        //get the emote panel
        emotePanel = transform.GetChild(3).gameObject;
        emoteText = emotePanel.transform.GetChild(0).GetComponent<Text>();

        PlayEmoteAudio(character);
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
            emoteClip = HelloVoices[character];
        }
        else if (emote == 1)
        {
            //wow
            emoteText.text = WowEmote;
            emoteClip = WowVoices[character];
        }
        else if (emote == 2)
        {
            //taunt
            emoteText.text = TauntEmote;
            emoteClip = TauntVoices[character];
        }
        voiceLineLength = emoteClip.length;

        //make sure it will be able to play
        if (emoteSource)
        {
            emoteSource.clip = emoteClip;
            //play the clip
            emoteSource.Play();
            //maybe also play an emote sound?
        }
    }

    IEnumerator AnimateLocalEmotePanel()
    {
        //activate the emote panel
        emotePanel.SetActive(true);

        //deactivate emotebutton
        Button emoteButton = transform.GetChild(3).GetChild(2).GetComponent<Button>();
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
