using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class MusicInfo : MonoBehaviour {

    public AudioSource musicSource;
    public AudioSource effectsSource;
    public AudioSource emotesSource;

    public AudioClip[] musicClips;
    public int clip = 0;
    private int currGameIndex = 1; 

    void Awake () {
		//to keep the same audio throughout, so doesn't restart music/ or reset volume
		GameObject[] objs = GameObject.FindGameObjectsWithTag("Audio");
		if (objs.Length > 1)
		{
			Destroy(this.gameObject);
		}
		DontDestroyOnLoad(this.gameObject);
	}

    private void Start()
    {
    }

    void Update()
    {
        if (PlayerPrefs.GetInt("muted", 0) == 1)
        {
            AudioListener.volume = 0;
        }
        else if(PlayerPrefs.GetInt("muted", 0) != 1)
        {
            AudioListener.volume = 1;
        }

        //get the volume levels from the player preferences
        musicSource.volume = PlayerPrefs.GetFloat("music", 1);
        effectsSource.volume = PlayerPrefs.GetFloat("effects", 1);
        emotesSource.volume = PlayerPrefs.GetFloat("emotes", 1);

        //switch to new track if ended
        if (!musicSource.isPlaying && currGameIndex != 6 && currGameIndex != 7 && currGameIndex != 8)
        {
            if (clip == 0)
            {
                clip = 1;
            }
            else
            {
                clip = 0;
            }
            musicSource.clip = musicClips[clip];
            musicSource.Play();
        }
    }

    public void ChangeMusic(int newGameIndex)
    {
        //don't restart music if reloading the same scene (ex. replaying a game)
        if (currGameIndex != newGameIndex)
        {
            if (newGameIndex == 6 || newGameIndex == 7 || newGameIndex == 8)
            {
                //play game music
                musicSource.clip = musicClips[2];
                musicSource.loop = true;
                musicSource.Play();
            }
            else if(currGameIndex == 6 || currGameIndex == 7 || currGameIndex == 8)
            {
                //play menu music when going from a game to menu
                clip = 0;
                musicSource.clip = musicClips[clip];
                musicSource.loop = false;
                musicSource.Play();
            }
        }
        currGameIndex = newGameIndex;
    }
}
