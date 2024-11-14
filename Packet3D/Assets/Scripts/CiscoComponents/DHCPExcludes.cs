using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class DHCPExcludes
{
    [Tooltip("Format: 192.168.1.1")]
    public string excludeBegin;
    [Tooltip("Format: 192.168.1.1")]
    public string excludeEnd;
}
