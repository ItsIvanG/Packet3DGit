using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CiscoEthernetPort : PortProperties
{
    public string network;
    public string networkSubnet;
    public string defaultRouter;
    public string dnsserver;

    public string excludeStart;
    public string excludeEnd;

    public enum switchportModes { None,Trunk,Access};
    public switchportModes switchportMode;
    public int switchportAccessVlan;

    public bool noShut = false;
}
