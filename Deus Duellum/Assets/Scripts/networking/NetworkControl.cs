using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using UnityEngine;
using UnityEngine.UI;
using Assets.Scripts;
using System;

public class NetworkControl : MonoBehaviour {

    public Client client;
    public Server server;
    public bool isClient;

    private GameObject serverstuff;
    private GameObject clientstuff;

    public GameCore _core;

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
        string name = PlayerPrefs.GetString("name", "test");

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
                Direction direction = ParseDirection(messages[3]);

                try
                {
                    _core.MakeNetworkMove(x, y, direction);
                }
                catch (Exception e)
                {
                    Debug.Log(e.Message);
                }
                break;
            case "emote":
                //call "OtherEmote" on player2's emotecontroller
                EmoteController player2Emotes = GameObject.FindGameObjectWithTag("Player2").GetComponent<EmoteController>();
                int emote = int.Parse(messages[1]);
                player2Emotes.OtherEmote(emote);
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
        serverstuff.transform.GetChild(1).GetComponent<Text>().text = "connection found";
        serverstuff.transform.GetChild(2).GetComponent<Button>().interactable = true;
    }

    public void SendMove(int x, int y, Direction direction)
    {
        string moveMessage = string.Format("move|{0}|{1}|{2}",x,y,direction);
        Send(moveMessage);
    }

    private Direction ParseDirection(string str)
    {
        switch (str)
        {
            case "West":
                return Direction.West;
            case "Forward":
                return Direction.Forward;
            case "East":
                return Direction.East;
        }

        //Should not reach here
        throw new System.Exception("Direction not in the correct format");
    }

    public void SetCore(GameCore core)
    {
        _core = core;
    }

    public void Disconnect()
    {
        if (isClient)
        {
            client.Disconnect();
        }
        else
        {
            server.Disconnect();
        }
    }

    public bool IsConnected()
    {
        if (isClient)
        {
            return client.IsConnected();
        }
        else
        {
            return server.IsConnected();
        }
    }
}
