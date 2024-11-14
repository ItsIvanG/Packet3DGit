using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vlan : CiscoEthernetPort
{
    public string vlanName;
    public int vlanNumber;
    public List<CiscoEthernetPort> vlanPorts;

    public void updateVlanPorts()
    {
        vlanPorts.Clear();
        var CiscoPorts = transform.parent.GetComponentsInChildren<CiscoEthernetPort>();
        foreach (var p in CiscoPorts)
        {
            if (p.switchportAccessVlan == vlanNumber)
            {
                vlanPorts.Add(p);
            }
        }
    }

    private void Start()
    {
        updateVlanPorts();
    }

}
