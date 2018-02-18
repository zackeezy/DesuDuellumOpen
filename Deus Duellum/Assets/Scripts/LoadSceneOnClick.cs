using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadSceneOnClick : MonoBehaviour {

    public int player = 1;

    public void LoadByIndex(int sceneIndex)
    {
        //do a cool scene transition

        SceneManager.LoadScene(sceneIndex);
    }
}
