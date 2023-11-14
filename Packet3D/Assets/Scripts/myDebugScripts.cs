using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class myDebugScripts : MonoBehaviour
{
    public bool updateHops=false;

    private void Update()
    {
        if (updateHops)
        {
            updateHops = false;
            UpdateAllHops();
        }
    }
    public void UpdateAllHops()
    {
        var hops = FindObjectsByType<CableHops>(0);
        foreach(var hop in hops)
        {
            hop.UpdateHops(hop.portA,hop.portB);
        }
    }
}
