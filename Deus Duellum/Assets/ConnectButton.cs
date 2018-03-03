using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ConnectButton : MonoBehaviour {

    public GameObject scrollViewControl;
    public NetworkControl networkControl;

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void ConnectWithServer()
    {
        scrollViewControl.GetComponent<ScrollViewScript>().Connected();

        networkControl.ServerSelected(int.Parse(scrollViewControl.GetComponent<ScrollViewScript>().serverNumber.text));
    }
}
