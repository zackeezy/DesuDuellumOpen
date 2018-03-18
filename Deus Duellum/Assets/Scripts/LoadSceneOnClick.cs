using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadSceneOnClick : MonoBehaviour {

    public int player = 1;

    public void LoadByIndex(int sceneIndex)
    {
        //do a cool scene transition
        AutoFade.LoadLevel(sceneIndex, 1, 1, Color.black);
        //SceneManager.LoadScene(sceneIndex);
    }

    public void LoadNetworkConnection(bool server)
    {
        if (server)
        {
            PlayerPrefs.SetInt("server", 1);
            AutoFade.LoadLevel(7, 1, 1, Color.black);

        }
        else
        {
            PlayerPrefs.SetInt("server", 0);
            AutoFade.LoadLevel(7, 1, 1, Color.black);

        }
    }
}
