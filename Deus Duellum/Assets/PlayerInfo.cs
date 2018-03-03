using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInfo {
    string ip;
    string name;

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
}
