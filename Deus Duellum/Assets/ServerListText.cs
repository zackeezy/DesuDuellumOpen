using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ServerListText : MonoBehaviour {

    public NetworkControl networkControl;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        PlayerInfo[] servers = networkControl.GetServerListFromClient();

        Text text = GetComponent<Text>();

        try
        {
            if (servers.Length == 0)
            {
                text.text = "No servers yet :(";
            }
            else
            {
                text.text = "";
                int i = 0;
                foreach (PlayerInfo pi in servers)
                {
                    text.text += (i++ + " " + pi.Name + " " + pi.IP + '\n');
                }
            }
        }
        catch (Exception e)
        {
            Debug.Log(e.Message);
        }
	}
}
