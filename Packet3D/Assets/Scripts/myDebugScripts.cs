using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class myDebugScripts : MonoBehaviour
{
    public bool updateHops=false;

    public static myDebugScripts instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

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
            hop.UpdateHops();
        }
    }

   
}
