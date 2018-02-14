using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Net;
using UnityEngine;
using UnityEngine.Networking;

public class Server : MonoBehaviour {

    int connectionId;
    int maxConnections = 2;
    int reliableChannelId;
    int hostId;
    int socketPort = 8888;
    byte error;

    int hostIdClient;
    int connectionIdClient;
    public GameObject clientObj;
    public GameObject player;

	// Use this for initialization
	void Start () {
        NetworkTransport.Init();
        ConnectionConfig config = new ConnectionConfig();
        reliableChannelId = config.AddChannel(QosType.ReliableSequenced);
        HostTopology topology = new HostTopology(config, maxConnections);
        hostId = NetworkTransport.AddHost(topology, socketPort, null);
        Debug.Log("Socket open. Host ID is: " + hostId);
	}
	
	// Update is called once per frame
	void Update () {
        int recvHostId;
        int recvConnectionId;
        int recvChannelId;
        byte[] recvBuffer = new byte[1024];
        int bufferSize = 1024;
        int datasize;

        NetworkEventType recvNetworkEvent = NetworkTransport.Receive(out recvHostId, out recvConnectionId, out recvChannelId, 
            recvBuffer, bufferSize, out datasize, out error);

        switch (recvNetworkEvent)
        {
            case NetworkEventType.ConnectEvent:
                //TODO: add code for starting game
                clientObj = Instantiate(player, transform.position, transform.rotation);
                clientObj.GetComponent<Player>().server = this;
                connectionIdClient = recvConnectionId;
                Debug.Log("ConnectEvent Triggered.");
                break;
            case NetworkEventType.DataEvent:
                string msg = Encoding.Unicode.GetString(recvBuffer, 0, datasize);
                Debug.Log("Receiving " + msg);
                string[] splitData = msg.Split('|');
                switch (splitData[0])
                {
                    case "MOVE":
                        //TODO: add code for move
                        Move(splitData[1], splitData[2], clientObj);
                        break;
                    case "EMOTE":
                        //TODO: add code for emote
                        break;
                    case "MESSAGE":

                        break;
                }
                break;
            case NetworkEventType.DisconnectEvent:

                break;
        }
	}

    public void Connect()
    {
        ConnectionConfig config = new ConnectionConfig();
        reliableChannelId = config.AddChannel(QosType.ReliableSequenced);
        HostTopology topology = new HostTopology(config, maxConnections);
        hostId = NetworkTransport.AddHost(topology, socketPort, "127.0.0.1");
        Debug.Log("Socket open. Host ID is: " + hostId);
        connectionId = NetworkTransport.Connect(hostId, "127.0.0.1", 8888, 0, out error);
    }

    public void Move(string x, string y, GameObject obj)
    {
        float xMov = float.Parse(x);
        float yMove = float.Parse(y);
        obj.transform.Translate(xMov, 0, yMove);
    }

    public void SendNetworkMessage(string message)
    {

        byte[] buffer = Encoding.Unicode.GetBytes(message);

        NetworkTransport.Send(hostIdClient, connectionIdClient, reliableChannelId, buffer, message.Length * sizeof(char), out error);
    }
}
