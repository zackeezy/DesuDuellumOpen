using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonScript : MonoBehaviour {

    public InputField inputField;
    public GameObject networkControl;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void Send()
    {
        string str = "MESSAGE|";
        str += inputField.text;
        networkControl.GetComponent<NetworkControl>().Send(str);
        networkControl.GetComponent<NetworkControl>().SentMessageUpdate(inputField.text);
    }
}
