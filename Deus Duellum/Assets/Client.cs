using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Net;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using System.Linq;
using System.Net.Sockets;
using System;
using System.Threading;

public class Client : MonoBehaviour {

    bool connected = false;

    int connectionId;
    int maxConnections = 2;
    int reliableChannelId;
    int hostId;
    int socketPort = 7778;
    int serverSocketPort = 9999;
    byte error;
    string recvIP;

    int hostIdServer;
    int connectionIdServer;
    public List<PlayerInfo> servers;

    public GameObject player;
    public GameObject networkControl;
    public InputField Name;

    //C# Networking variables
    bool csharpconnected = false;
    UdpClient listener;
    IPEndPoint groupEP;
    string received_data;
    byte[] receive_byte_array;
    Thread receiveThread;
    int multicastPort = 10101;
    List<PlayerInfo> serverList;

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
        
        IPAddress ip = IPAddress.Parse("224.5.6.7");
        //groupEP = new IPEndPoint(IPAddress.Any, socketPort);
        received_data = "";
        listener = new UdpClient();
        IPEndPoint ipep = new IPEndPoint(IPAddress.Any, multicastPort);
        listener.Client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);
        listener.Client.Bind(ipep);
        listener.JoinMulticastGroup(ip);
        receive_byte_array = new byte[1024];
        receiveThread = new Thread(ReceiveData);
        //AutoResetEvent are = new AutoResetEvent(false);
        //Timer t = new Timer(ReceiveData, are, 1000, 2000);
        serverList = new List<PlayerInfo>();
    }

    // Update is called once per frame
    void Update()
    {
        if (csharpconnected) {
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
                        //case "CONNECT":
                        //    PlayerInfo pi = new PlayerInfo()
                        //    {
                        //        IP = splitData[1],
                        //        ConnectionId = int.Parse(splitData[2]),
                        //        HostId = int.Parse(splitData[3]),
                        //        Name = splitData[4]
                        //    };
                        //    if (!servers.Contains(pi))
                        //    {
                        //        servers.Add(pi);
                        //        Debug.Log(pi.Name + " at " + pi.IP + " was received");
                        //    }
                        //    Show list of Servers
                        //    GameObject.Find("ScrollView").GetComponent<ScrollViewScript>().PopulateServers();
                        //    break;
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
        else
        {
            if (receiveThread.ThreadState == ThreadState.Unstarted)
                receiveThread.Start();
            //listener.Close();
        }
    }

    public void Connect()
    {
        NetworkTransport.Init();
        ConnectionConfig config = new ConnectionConfig();
        reliableChannelId = config.AddChannel(QosType.ReliableSequenced);
        HostTopology topology = new HostTopology(config, maxConnections);
        hostId = NetworkTransport.AddHost(topology, socketPort);
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

    void ReceiveData(/*IAsyncResult ar*/)
    {
        while (true)
        {
            try
            {
                Byte[] data = listener.Receive(ref groupEP);
                string strData = Encoding.ASCII.GetString(data);
                Debug.Log(strData);
                string[] splitData = strData.Split('|');
                PlayerInfo server = new PlayerInfo()
                {
                    Name = splitData[0],
                    IP = splitData[1]
                };
                Debug.Log("Server added: " + server.Name + " " + server.IP);
                AddServer(server);
            }
            catch (Exception e)
            {
                Debug.Log(e.GetType().ToString());
                Debug.Log(e.ToString());
            }
            Thread.Sleep(1000);
        }
    }

    public void ServerSelected(int index)
    {
        
    }

    private void OnApplicationQuit()
    {
        receiveThread.Abort();
        listener.Close();
    }

    private bool AddServer(PlayerInfo server)
    {
        bool val = true;
        if (serverList.Count != 0)
        {
            serverList.ForEach((s) =>
            {
                if (server.IP == s.IP)
                {
                    val = false;
                }
            });
        }
        if (val)
        {
            serverList.Add(server);
        }
        return val;
    }

    public PlayerInfo[] GetServers()
    {
        PlayerInfo[] list = new PlayerInfo[serverList.Count];
        serverList.CopyTo(list);
        return list;
    }
}
