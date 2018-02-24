using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInfo {
    string ip;
    string name;
    int connectionId;
    int hostId;

    public string IP
    {
        get
        {
            return ip;
        }
        set
        {
            ip = value;
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

    public int ConnectionId
    {
        get
        {
            return connectionId;
        }

        set
        {
            connectionId = value;
        }
    }

    public int HostId
    {
        get
        {
            return hostId;
        }

        set
        {
            hostId = value;
        }
    }
}
