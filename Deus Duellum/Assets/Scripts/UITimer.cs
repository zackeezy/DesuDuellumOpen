//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using UnityEngine.UI;

//public class UITimer : MonoBehaviour {

//    public int timeLeft = 60;
//    public Text countdownText;

//	// Use this for initialization
//	void Start () {
//        StartCoroutine("LoseTime");
//	}
	
//	// Update is called once per frame
//	void Update () {
//        countdownText.text = ("Time Left: \n " + timeLeft+" seconds");

//        if (timeLeft <= 0)
//        {
//            StopCoroutine("LoseTime");
//            countdownText.text = "Time's up!";
//        }
//	}

//    IEnumerator LoseTime()
//    {
//        while (true)
//        {
//            yield return new WaitForSeconds(1);
//            timeLeft--;
//        }
//    }
//}
