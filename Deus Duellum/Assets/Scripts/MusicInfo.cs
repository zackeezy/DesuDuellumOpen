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
            if (clip >= 3)
            {
                clip = 0;
            }
            musicSource.clip = musicClips[clip];
            musicSource.Play();
        }
    }

    public void ChangeMusic(int gameIndex)
    {
        if(gameIndex == 6 || gameIndex == 7 || gameIndex == 8)
        {
            //play game music
            musicSource.clip = musicClips[3];
        }
        else
        {
            //play menu music
            musicSource.clip = musicClips[clip];
        }
        musicSource.Play();
    }
}
