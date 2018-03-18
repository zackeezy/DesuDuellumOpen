using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using UnityEngine;
using UnityEngine.UI;

public class NetworkControl : MonoBehaviour {

    public Client client;
    public Server server;
    public bool isClient;

    private GameObject serverstuff;
    private GameObject clientstuff;

    public BoardManager manager;

    void Awake()
    {
        //to keep the same networkcontroller throughout
        GameObject[] objs = GameObject.FindGameObjectsWithTag("network");
        if (objs.Length > 1)
        {
            Destroy(this.gameObject);
        }
        DontDestroyOnLoad(this.gameObject);
    }

    // Use this for initialization
    void Start () {
        int isserver = PlayerPrefs.GetInt("server", 1);
        string name = "test";

        serverstuff = GameObject.FindGameObjectWithTag("server");
        clientstuff = GameObject.FindGameObjectWithTag("client");

        if (isserver == 1)
        {
            InitializeAs(false, name);
            serverstuff.SetActive(true);
            clientstuff.SetActive(false);
        }
        else
        {
            InitializeAs(true, name);
            serverstuff.SetActive(false);
            clientstuff.SetActive(true);
        }

        manager = GameObject.FindGameObjectWithTag("boardManager").GetComponent<BoardManager>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void Send(string s)
    {
        if (isClient)
        {
            client.SendNetworkMessage(s);
        }
        else
        {
            server.SendNetworkMessage(s);
        }
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

    public PlayerInfo[] GetServerListFromClient()
    {
        return client.GetServers();
    }

    public void ServerSelected(int index)
    {
        client.ServerSelected(index);
    }

    public void ParseMessage(string[] messages)
    {
        switch (messages[0])
        {
            case "character":
                //they have selected a character
                GameObject options = GameObject.FindGameObjectWithTag("options");
                if (options)
                {
                    CharacterSelect select = options.GetComponent<CharacterSelect>();
                    int character = int.Parse(messages[1]);
                    select.OtherPlayerSelected(character);
                }             
                break;
            case "move":
                int x = int.Parse(messages[1]), y = int.Parse(messages[2]);
                float xPos = float.Parse(messages[3]), zPos = float.Parse(messages[4]);
                bool success = manager.AttemptMove(x, y, xPos, zPos);
                break;
            case "emote":

                break;

        }

    }

    public void InitializeAs(bool isClient, string name)
    {
        this.isClient = isClient;
        if (isClient)
        {
            server = null;
            client = gameObject.AddComponent(typeof(Client)) as Client;
            client.Name = name;
            client.networkControl = this;
        }
        else
        {
            client = null;
            server = gameObject.AddComponent(typeof(Server)) as Server;
            server.Name = name;
            server.networkControl = this;
            
        }
    }

    public void ServerConnected()
    {
        serverstuff.transform.GetChild(0).GetComponent<Text>().text = "connection found";
        serverstuff.transform.GetChild(1).GetComponent<Button>().interactable = true;
    }

    public void SendMove(int x, int y, float xPos, float zPos)
    {
        string moveMessage = string.Format("move|{0}|{1}|{2}|{3}",x,y,xPos,zPos);
        Send(moveMessage);
    }
}
