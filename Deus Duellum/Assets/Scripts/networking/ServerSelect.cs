using System;
using System.Collections;
using System.Collections.Generic;
//using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class ServerSelect : MonoBehaviour {

    public NetworkControl netcontroller;
    public Text descText;
    public Text selectionText;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void ServerSelection()
    {
        //string servername = GetComponent<InputField>().text;
        string servername = selectionText.text;
        bool serverFound = false;
        if (servername != "")
        {
            try
            {
                PlayerInfo[] servers = netcontroller.GetServerListFromClient();
                int index = 0;
                foreach (PlayerInfo info in servers)
                {
                    if (servername == info.Name)
                    {
                        netcontroller.ServerSelected(index);
                        serverFound = true;
                        LoadSceneOnClick scenechanger = GetComponent<LoadSceneOnClick>();
                        scenechanger.LoadByIndex(4);
                        break;
                    }
                    index++;
                }
            }
            catch(Exception e)
            {
                Debug.Log(e.Message);
            }
        }
        if (!serverFound)
        {
            //invalid name, must choose another
            descText.text = "Please enter a valid name";
            GetComponent<InputField>().text = "";
        }
    }

    public void NameEntered(GameObject obj)
    {
        Button playButton = obj.GetComponent<Button>();
        string servername = GetComponent<InputField>().text;
        if (playButton)
        {
            if (servername != "")
            {
                playButton.interactable = true; ;
            }
            else 
            {
                playButton.interactable = false;
            }
        }
    }
}
