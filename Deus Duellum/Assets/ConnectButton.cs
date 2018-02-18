using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ConnectButton : MonoBehaviour {

    public ScrollRect serverList;
    public Text selectServerText;
    public Button serverSelectButton;
    public NetworkControl networkControl;

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void ConnectWithServer()
    {


        networkControl.client.GetComponent<Client>().Connect();
    }
}
