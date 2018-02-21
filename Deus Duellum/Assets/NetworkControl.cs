using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using UnityEngine;
using UnityEngine.UI;

public class NetworkControl : MonoBehaviour {

    public GameObject client;
    public GameObject server;
    public bool isClient;

    public Text you;
    public Text them;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void Send(string s)
    {
        if (isClient)
        {
            client.GetComponent<Client>().SendNetworkMessage(s);
        }
        else
        {
            server.GetComponent<Server>().SendNetworkMessage(s);
        }
    }

    public void Receive(string recvStr)
    {
        them.text = recvStr;
    }

    public void SentMessageUpdate(string s)
    {
        you.text = s;
    }

    public static IPAddress LocalIPAddress()
    {
        if (!System.Net.NetworkInformation.NetworkInterface.GetIsNetworkAvailable())
        {
            return null;
        }

        IPHostEntry host = Dns.GetHostEntry(Dns.GetHostName());

        return host
            .AddressList
            .FirstOrDefault(ip => ip.AddressFamily == AddressFamily.InterNetwork);
    }
}
