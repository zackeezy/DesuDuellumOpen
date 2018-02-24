using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Net.Sockets;
using UnityEngine;
using UnityEngine.Networking;

public class Server : /*MonoBehaviour*/ NetworkDiscovery
{

    bool connected = false;

    int connectionId;
    int maxConnections = 2;
    int reliableChannelId;
    int hostId;
    int socketPort = 8888;
    byte error;
    int clientPort;
    string myIP;

    string clientIP;

    int hostIdClient;
    int connectionIdClient;
    public GameObject clientObj;
    public GameObject player;

    public GameObject networkControl;

    public string ClientIP
    {
        get
        {
            return clientIP;
        }

        set
        {
            clientIP = value;
        }
    }

    // Use this for initialization
    void Start () {
        clientObj = null;
        NetworkTransport.Init();
        ConnectionConfig config = new ConnectionConfig();
        reliableChannelId = config.AddChannel(QosType.ReliableSequenced);
        HostTopology topology = new HostTopology(config, maxConnections);
        hostId = NetworkTransport.AddHost(topology, socketPort);
        Debug.Log("Socket open. Host ID is: " + hostId);
        myIP = NetworkControl.LocalIPAddress().ToString();

        UdpClient udpClient = new UdpClient(8888);
        udpClient.Send(Encoding.ASCII.GetBytes(":p"), 2, IPAddress.Broadcast.ToString(), 8888);
        udpClient.Close();
    }
	
	// Update is called once per frame
	void Update () {
        int recvHostId;
        int recvConnectionId;
        int recvChannelId;
        byte[] recvBuffer = new byte[1024];
        int bufferSize = 1024;
        int datasize;

        NetworkEventType recvNetworkEvent = NetworkTransport.Receive(out recvHostId, 
            out recvConnectionId, out recvChannelId, recvBuffer, bufferSize, 
            out datasize, out error);

        switch (recvNetworkEvent)
        {
            case NetworkEventType.ConnectEvent:
                //TODO: add code for starting game
                clientObj = Instantiate(player, transform.position, transform.rotation);
                connectionIdClient = recvConnectionId;
                hostIdClient = recvHostId;
                clientObj.GetComponent<Player>().networkControl = networkControl;
                Debug.Log("ConnectEvent Triggered.");
                connected = true;
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
                        networkControl.GetComponent<NetworkControl>().Receive(splitData[1]);
                        break;
                }
                break;
            case NetworkEventType.DisconnectEvent:
                clientObj = null;

                break;
                //case NetworkEventType.BroadcastEvent:
                //    Debug.Log("BroadcastEvent Triggered");
                //    string broadcastMsg = "";
                //    byte[] buffer = new byte[1024];
                //    int recvsize;
                //    byte error;

                //    NetworkTransport.GetBroadcastConnectionInfo(hostId, out clientIP, out clientPort, out error);

                //    NetworkTransport.GetBroadcastConnectionMessage(hostId, buffer, 1024, out recvsize, out error);

                //    broadcastMsg = Encoding.Default.GetString(buffer);

                //    Connect();

                //    string response = "CONNECT|" + myIP + "|" + connectionId + "|" + hostId + "|Server";

                //    NetworkTransport.Send(hostId, connectionId, reliableChannelId, Encoding.ASCII.GetBytes(response),
                //        response.Length * sizeof(char), out error);

                //    break;
        }
	}

    public void Connect()
    {
        connectionId = NetworkTransport.Connect(hostId, ClientIP, clientPort, 0, out error);
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
        NetworkTransport.Send(hostIdClient, connectionIdClient, reliableChannelId, 
            buffer, message.Length * sizeof(char), out error);
    }

    public override void OnReceivedBroadcast(string fromAddress, string data)
    {
        Debug.Log("BroadcastEvent Triggered");
        string broadcastMsg = "";
        byte[] buffer = new byte[1024];
        int recvsize;
        byte error;

        NetworkTransport.GetBroadcastConnectionInfo(hostId, out clientIP, out clientPort, out error);

        NetworkTransport.GetBroadcastConnectionMessage(hostId, buffer, 1024, out recvsize, out error);

        broadcastMsg = Encoding.Default.GetString(buffer);

        clientIP = broadcastMsg.Split('|')[1];

        clientPort = int.Parse(broadcastMsg.Split('|')[2]);

        connectionIdClient = int.Parse(broadcastMsg.Split('|')[3]);

        hostIdClient = int.Parse(broadcastMsg.Split('|')[4]);

        Connect();

        string response = "CONNECT|" + myIP + "|" + connectionId + "|" + hostId + "|Server";

        NetworkTransport.Send(hostIdClient, connectionIdClient, reliableChannelId, Encoding.ASCII.GetBytes(response),
            response.Length * sizeof(char), out error);

        base.OnReceivedBroadcast(fromAddress, data);
    }
}
