using System.Collections;
using System.Collections.Generic;
//using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class ServerSelect : MonoBehaviour {

    public NetworkControl netcontroller;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void ServerSelection()
    {
        string servername = GetComponent<InputField>().text;
        if (servername != "")
        {
            PlayerInfo[] servers = netcontroller.GetServerListFromClient();

            int index = 0;
            foreach (PlayerInfo info in servers)
            {
                if (servername == info.Name)
                {
                    netcontroller.ServerSelected(index);
                    LoadSceneOnClick scenechanger = GetComponent<LoadSceneOnClick>();
                    scenechanger.LoadByIndex(2);
                    break;
                }
                index++;
            }
        }
    }
}
