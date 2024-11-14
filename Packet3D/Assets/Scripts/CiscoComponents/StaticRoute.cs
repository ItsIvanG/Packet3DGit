using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class StaticRoute 
{
    [Tooltip("Format: 192.168.1.1/24")]
    public string network;
    [Tooltip("Format: 192.168.1.1")]
    public string route;
}
