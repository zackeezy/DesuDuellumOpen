using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class Intro_To_Menu : MonoBehaviour {

    public VideoPlayer player;
    public double playTime;
    public double currentTime;

    void Start()
    { 
        playTime = player.clip.length;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.anyKey)
        {
            AutoFade.LoadLevel(1, 1, 1, Color.black);
            //SceneManager.LoadScene(1);
        }

        currentTime = player.time;
        if (currentTime >= playTime)
        {
            AutoFade.LoadLevel(1, 1, 1, Color.black);
            //SceneManager.LoadScene(1);
        }
    }
}
