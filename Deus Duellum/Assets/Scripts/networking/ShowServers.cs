using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShowServers : MonoBehaviour {

    public NetworkControl netcontroller;
    //public GameObject content;
    public Text serverlist;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        PlayerInfo[] servers = netcontroller.GetServerListFromClient();

        serverlist.text = "";

        foreach(PlayerInfo info in servers)
        {
            serverlist.text += info.Name + "\n";
        }
	}
}
