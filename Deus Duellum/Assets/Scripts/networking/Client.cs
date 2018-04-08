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
    new string name;

    int hostIdServer;
    int connectionIdServer;
    public List<PlayerInfo> servers;

    //public GameObject player;
    public NetworkControl networkControl;

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

    public string Name
    {
        get
        {
            return name;
        }

        set
        {
            name = value;
        }
    }

    // Use this for initialization
    void Start()
    {
        NetworkTransport.Init();
        servers = new List<PlayerInfo>();
        
        IPAddress ip = IPAddress.Parse("224.5.6.7");
        received_data = "";
        IPEndPoint localEP = new IPEndPoint(NetworkControl.LocalIPAddress(), multicastPort);
        listener = new UdpClient();
        listener.Client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);
        listener.Client.Bind(localEP);
        listener.JoinMulticastGroup(ip);
        receive_byte_array = new byte[1024];
        receiveThread = new Thread(ReceiveData);
        serverList = new List<PlayerInfo>();
        receiveThread.Start();
    }

    // Update is called once per frame
    void Update()
    {
        
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
                networkControl.ParseMessage(splitData);
                break;
        }
    }

    public void Connect()
    {
        ConnectionConfig config = new ConnectionConfig();
        reliableChannelId = config.AddChannel(QosType.ReliableSequenced);
        HostTopology topology = new HostTopology(config, maxConnections);
        hostId = NetworkTransport.AddHost(topology, socketPort);
        connectionId = NetworkTransport.Connect(hostId, RecvIP, serverSocketPort, 0, out error);
        NetworkError networkError = (NetworkError)error;
        if (networkError != NetworkError.Ok)
        {
            Debug.LogError(string.Format("Unable to connect to {0}:{1}, Error: {2}", hostId, serverSocketPort, networkError));
        }
        else
        {
            Debug.Log(string.Format("Connected to {0}:{1} with hostId: {2}, connectionId: {3}, channelId: {4},", hostId, serverSocketPort, hostId, connectionId, reliableChannelId));
        }
        connected = true;
    }

    public void Disconnect()
    {
        NetworkTransport.Disconnect(hostId, connectionId, out error);
    }

    public void SendNetworkMessage(string message)
    {
        byte[] buffer = Encoding.Unicode.GetBytes(message);
        NetworkTransport.Send(hostId, connectionId, reliableChannelId, buffer, message.Length * sizeof(char), out error);
        NetworkError networkError = (NetworkError)error;
        if (networkError != NetworkError.Ok)
        {
            Debug.LogError(string.Format("Error: {0}, hostId: {1}, connectionId: {2}, channelId: {3}", error, hostId, connectionId, reliableChannelId));
        }
        else
        {
            Debug.Log("Message sent!");
        }
    }

    void ReceiveData()
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
        PlayerInfo selected = serverList[index];

        recvIP = selected.IP;

        receiveThread.Abort();

        Connect();
    }

    private void OnApplicationQuit()
    {
        if (receiveThread.IsAlive)
        {
            receiveThread.Abort();
        }
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
            Debug.Log("Server added: " + server.Name + " " + server.IP);
            serverList.Add(server);
        }
        return val;
    }

    public PlayerInfo[] GetServers()
    {
        PlayerInfo[] list = new PlayerInfo[serverList.Count];
        serverList.CopyTo(list);
        serverList.Clear();
        return list;
    }

    public bool IsConnected()
    {
        return connected;
    }
}
