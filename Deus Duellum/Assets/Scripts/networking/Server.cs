using System.Text;
using System.Net;
using System.Net.Sockets;
using UnityEngine;
using UnityEngine.Networking;
using System.Threading;

public class Server : MonoBehaviour 
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
    new string name;

    string clientIP;

    int hostIdClient;
    int connectionIdClient;

    public NetworkControl networkControl;

    //C# networking stuff
    UdpClient sendingSocket;
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
    void Start () {
        NetworkTransport.Init();
        myIP = NetworkControl.LocalIPAddress().ToString();

        sendingSocket = new UdpClient();
        IPAddress ip = IPAddress.Parse("224.5.6.7");
        sendingSocket.JoinMulticastGroup(ip, 32);
        ipep = new IPEndPoint(ip, multicastPort);

        sendByteArray = Encoding.ASCII.GetBytes(name + "|" + 
            NetworkControl.LocalIPAddress().ToString());

        ConnectionConfig config = new ConnectionConfig();
        reliableChannelId = config.AddChannel(QosType.ReliableSequenced);
        HostTopology topology = new HostTopology(config, maxConnections);

        hostId = NetworkTransport.AddHost(topology, socketPort);
        Debug.Log("Socket open. Host ID is: " + hostId);

        InvokeRepeating("PingClient", 0, 1);
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
                //code for starting game
                if (!connected)
                {
                    connectionIdClient = recvConnectionId;
                    hostIdClient = recvHostId;
                    Debug.Log("ConnectEvent Triggered.");
                    connected = true;
                    networkControl.ServerConnected();
                }
                else if(recvConnectionId != connectionIdClient || recvHostId != hostIdClient)
                {
                    NetworkTransport.Disconnect(recvHostId, recvConnectionId, out error);
                }
                break;
            case NetworkEventType.DataEvent:
                string msg = Encoding.Unicode.GetString(recvBuffer, 0, datasize);
                Debug.Log("Receiving " + msg);
                string[] splitData = msg.Split('|');
                networkControl.ParseMessage(splitData);
                break;
            case NetworkEventType.DisconnectEvent:
                connected = false;
                networkControl.GameTimedOut();
                break;
        }
        if (connected)
        {
            CancelInvoke();
        }
    }

    public void Connect()
    {
        ConnectionConfig config = new ConnectionConfig();
        reliableChannelId = config.AddChannel(QosType.ReliableSequenced);
        HostTopology topology = new HostTopology(config, maxConnections);
        hostId = NetworkTransport.AddHost(topology, socketPort);
        Debug.Log("Socket open. Host ID is: " + hostId);
        connectionId = NetworkTransport.Connect(hostId, ClientIP, clientPort, 0, out error);
        connected = true;
    }

    public void SendNetworkMessage(string message)
    {
        byte[] buffer = Encoding.Unicode.GetBytes(message);
        NetworkTransport.Send(hostIdClient, connectionIdClient, reliableChannelId, buffer, message.Length * sizeof(char), out error);
    }

    void WaitForResponse()
    {
        Debug.Log("Response thread Started...");
        UdpClient waiter = new UdpClient();
        IPEndPoint ipep = new IPEndPoint(IPAddress.Any, clientPort);
        byte[] response = waiter.Receive(ref ipep);
        Debug.Log("Response received: " + response.ToString());

        string responseStr = Encoding.ASCII.GetString(response);
        string IPAddressStr = responseStr.Split('|')[1];

        ClientIP = IPAddressStr;

        Connect();
    }

    public void Disconnect()
    {
        NetworkTransport.Disconnect(hostId, connectionId, out error);
    }

    public bool IsConnected()
    {
        return connected;
    }

    private void OnDestroy()
    {
        Debug.Log("Server OnDestroy");
        NetworkTransport.RemoveHost(hostId);
        sendingSocket.Close();
    }

    private void PingClient()
    {
        if (!connected)
        {
            Debug.Log("Sending server name and IP");
            sendingSocket.Send(sendByteArray, sendByteArray.Length, ipep);
        }
    }
}
