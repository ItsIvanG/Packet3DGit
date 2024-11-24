using System;
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

    public enum switchportModes { None, Trunk, Access };
    public switchportModes switchportMode;
    public int switchportAccessVlan;


    public bool noShut
    {
        get => _noShut;
        set
        {
            if (_noShut != value)
            {
                _noShut = value;
                OnNoShutChange(value);
            }
        }
    }
    public bool _noShut = false;

    public event Action<bool> noShutChanged;

    public bool switchport = false;

    private void OnNoShutChange(bool newValue)
    {
        Debug.Log($"WatchedBool changed to: {newValue}");
        noShutChanged?.Invoke(newValue);
        if (transform.Find("Lights"))
        {
            Renderer lightRender= transform.Find("Lights").GetComponentInChildren<Renderer>();
            if (lightRender)
            {
                Material mat = lightRender.material;
            mat.SetColor("_EmissionColor", new Color(0, 50, 0) * (1 * (newValue ? 1 : 0)));
            }
        }
    }

    void Start()
    {
        // Subscribe to the BoolChanged event
        noShutChanged += HandleNoShutChange;
    }
    private void HandleNoShutChange(bool newValue)
    {
        Debug.Log($"BoolChangeListener detected change: {newValue}");
    }

    private void OnValidate()
    {
        // Sync the WatchedBool property with the serialized field
        noShut = _noShut;
        //OnNoShutChange(noShut);
    }
}
