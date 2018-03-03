using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Net.Sockets;
using UnityEngine;
using UnityEngine.Networking;
using System;
using System.Threading;

public class Server : MonoBehaviour /*NetworkDiscovery*/
{

    bool connected = false;

    int connectionId;
    int maxConnections = 2;
    int reliableChannelId;
    int hostId;
    int socketPort = 9999;
    byte error;
    int clientPort = 7778;
    string myIP;

    string clientIP;

    int hostIdClient;
    int connectionIdClient;
    public GameObject clientObj;
    public GameObject player;

    public GameObject networkControl;

    //C# networking stuff
    bool csharpconnected = false;
    UdpClient sendingSocket;
    IPAddress sendToAddress;
    IPEndPoint sendingEndPoint;
    Thread sendThread;
    bool broadcasting = true;
    byte[] sendByteArray;
    int multicastPort = 10101;
    IPEndPoint ipep;

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
        NetworkTransport.Init();
        clientObj = null;
        myIP = NetworkControl.LocalIPAddress().ToString();

        sendingSocket = new UdpClient();
        IPAddress ip = IPAddress.Parse("224.5.6.7");
        //sendToAddress = IPAddress.Broadcast;
        //sendingEndPoint = new IPEndPoint(IPAddress.Broadcast, clientPort);
        //Debug.Log(IPAddress.Broadcast.ToString());
        sendingSocket.JoinMulticastGroup(ip);
        ipep = new IPEndPoint(ip, multicastPort);

        sendByteArray = Encoding.ASCII.GetBytes("Server|" + NetworkControl.LocalIPAddress().ToString());

        //sendThread = new Thread(Broadcast);
        //sendThread.Start();
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
                if (!connected)
                {
                    clientObj = Instantiate(player, transform.position, transform.rotation);
                    connectionIdClient = recvConnectionId;
                    hostIdClient = recvHostId;
                    clientObj.GetComponent<Player>().networkControl = networkControl;
                    Debug.Log("ConnectEvent Triggered.");
                    Connect();
                    connected = true;
                }
                if(connected && recvConnectionId != connectionIdClient)
                {
                    NetworkTransport.Disconnect(recvHostId, recvConnectionId, out error);
                }
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
                csharpconnected = false;
                break;
        }
        if (!csharpconnected)
        {
            sendingSocket.Send(sendByteArray, sendByteArray.Length, ipep);
        }
    }

    public void Connect()
    {
        ConnectionConfig config = new ConnectionConfig();
        reliableChannelId = config.AddChannel(QosType.ReliableSequenced);
        HostTopology topology = new HostTopology(config, maxConnections);
        hostId = NetworkTransport.AddHost(topology, socketPort);
        Debug.Log("Socket open. Host ID is: " + hostId);
        //connectionId = NetworkTransport.Connect(hostId, ClientIP, clientPort, 0, out error);
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

    private void OnApplicationQuit()
    {
        //sendThread.Abort();
        sendingSocket.Close();
    }
}
