using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class MusicInfo : MonoBehaviour {

    public AudioSource musicSource;
    public AudioSource effectsSource;
    public AudioSource emotesSource;

    public AudioClip[] musicClips;
    private int clip = 0;
    private int prevGameIndex = 1; 

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
        if (!musicSource.isPlaying)
        {
            clip++;
            if (clip >= musicClips.Length-1)
            {
                clip = 0;
            }
            musicSource.clip = musicClips[clip];
            musicSource.Play();
        }
    }

    public void ChangeMusic(int gameIndex)
    {
        //don't restart music if reloading the same scene (ex. replaying a game)
        if (prevGameIndex != gameIndex)
        {
            if (gameIndex == 6 || gameIndex == 7 || gameIndex == 8)
            {
                //play game music
                musicSource.clip = musicClips[2];
                musicSource.loop = true;
                musicSource.Play();
            }
            else if(prevGameIndex == 6 || prevGameIndex == 7 || prevGameIndex == 8)
            {
                //play menu music when going from a game to menu
                musicSource.clip = musicClips[clip];
                musicSource.loop = false;
                musicSource.Play();
            }
        }
        prevGameIndex = gameIndex;
    }
}
