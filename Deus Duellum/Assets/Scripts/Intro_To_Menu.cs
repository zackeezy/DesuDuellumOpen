using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Intro_To_Menu : MonoBehaviour {
    void Start()
    {
        StartCoroutine("DelayTransition");
    }
    IEnumerator DelayTransition()
    {
        int delayScreenTransition = 7;
        yield return new WaitForSeconds(delayScreenTransition);
        SceneManager.LoadScene(1);
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.anyKey)
        {
            SceneManager.LoadScene(1);
        }
    }
}
