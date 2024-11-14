using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DHCPPool
{
    public string Name;
    [Tooltip("Format: 192.168.1.1/24")]
    public string network;
    [Tooltip("Format: 192.168.1.1")]
    public string defaultGateway;
    [Tooltip("Format: 192.168.1.1")]
    public string defaultDNS;

}
