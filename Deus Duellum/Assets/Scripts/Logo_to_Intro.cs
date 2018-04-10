using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Logo_to_Intro : MonoBehaviour {
	// Use this for initialization
	void Start () {
		StartCoroutine ("DelayTransition");
	}
	IEnumerator DelayTransition()
	{
		int delayScreenTransition = 2;
		yield return new WaitForSeconds(delayScreenTransition);
		SceneManager.LoadScene(9);
	}
	// Update is called once per frame
	void Update () {
		if (Input.anyKey)
		{
			SceneManager.LoadScene(9);
		}	
	}
}
