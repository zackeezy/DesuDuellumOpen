using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Net;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using System.Linq;
using System.Net.Sockets;

public class Client : MonoBehaviour {

    bool connected = false;

    int connectionId;
    int maxConnections = 2;
    int reliableChannelId;
    int hostId;
    int socketPort = 7778;
    int serverSocketPort = 8888;
    byte error;
    string recvIP;

    int hostIdServer;
    int connectionIdServer;
    public List<PlayerInfo> servers;

    public GameObject player;
    public GameObject networkControl;
    public InputField Name;

    public string RecvIP
    {
        get
        {
            return recvIP;
        }

        set
        {
            recvIP = value;
        }
    }

    // Use this for initialization
    void Start()
    {
        servers = new List<PlayerInfo>();
        NetworkTransport.Init();
        ConnectionConfig config = new ConnectionConfig();
        reliableChannelId = config.AddChannel(QosType.ReliableSequenced);
        HostTopology topology = new HostTopology(config, maxConnections);
        hostId = NetworkTransport.AddHost(topology, socketPort);
        Debug.Log("Socket open. Host ID is: " + hostId);


        UdpClient udpClient = new UdpClient(7778);
        udpClient.Send(Encoding.ASCII.GetBytes(":p"), 2, IPAddress.Broadcast.ToString(), 7778);
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
            out recvConnectionId, out recvChannelId, recvBuffer, bufferSize, out datasize, out error);

        switch (recvNetworkEvent)
        {
            case NetworkEventType.ConnectEvent:
                if (!connected)
                {
                    connectionIdServer = recvConnectionId;
                    hostIdServer = recvHostId;
                    connected = true;
                }
                break;
            case NetworkEventType.DisconnectEvent:
                connected = false;
                break;
            case NetworkEventType.DataEvent:
                string msg = Encoding.Unicode.GetString(recvBuffer, 0, datasize);
                Debug.Log("Receiving " + msg);
                string[] splitData = msg.Split('|');
                switch (splitData[0])
                {
                    case "CONNECT":
                        PlayerInfo pi = new PlayerInfo()
                        {
                            IP = splitData[1],
                            ConnectionId = int.Parse(splitData[2]),
                            HostId = int.Parse(splitData[3]),
                            Name = splitData[4]
                        };
                        if (!servers.Contains(pi))
                        {
                            servers.Add(pi);
                            Debug.Log(pi.Name + " at " + pi.IP + " was received");
                        }
                        //Show list of Servers
                        GameObject.Find("ScrollView").GetComponent<ScrollViewScript>().PopulateServers();
                        break;
                    case "MOVE":
                        //TODO: add code for move
                        Move(splitData[1], splitData[2], player);
                        break;
                    case "EMOTE":
                        //TODO: add code for emote
                        break;
                    case "MESSAGE":
                        //Won't be used in final version 
                        networkControl.GetComponent<NetworkControl>().Receive(splitData[1]);
                        break;
                }
                break;
        }
    }

    public void Connect()
    {
        connectionId = NetworkTransport.Connect(hostId, RecvIP, serverSocketPort, 0, out error);
    }

    public void Disconnect()
    {
        NetworkTransport.Disconnect(hostId, connectionId, out error);
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
        NetworkTransport.Send(hostId, connectionId, reliableChannelId, buffer, message.Length * sizeof(char), out error);
    }

    public void Broadcast()
    {
        byte[] send = Encoding.ASCII.GetBytes(Name.text == "" ? "Default" : Name.text + '|' + 
            NetworkControl.LocalIPAddress().ToString() + "|" + socketPort);
        NetworkTransport.StartBroadcastDiscovery(hostId, socketPort, 2222, 1, 1, send, send.Length, 2000, out error);
    }

    public void ServerSelected(int index)
    {
        recvIP = servers[index].IP;

        servers.FindAll((server) => server.IP != servers[index].IP || server.Name != servers[index].Name)
            .ForEach((server) =>
        {
            NetworkTransport.Disconnect(server.HostId, server.ConnectionId, out error);
        });

        NetworkTransport.StopBroadcastDiscovery();
    }
}
