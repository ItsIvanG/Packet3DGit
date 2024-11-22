using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;


public class SimulationBehavior : MonoBehaviour
{
    public static SimulationBehavior instance;
    private GameObject tempApick;
    private GameObject tempBpick;
    public GameObject APick;
    public GameObject BPick;
    public TMP_Dropdown A_Dropdown;
    public TMP_Dropdown B_Dropdown;
    public List<GameObject> DevicesList;
    public List<string> DevicesNames;
    public Toggle speed0_5x;
    public Toggle speed1x;
    public Toggle speed2x;
    public bool isPicking;
    public int pickingWhat = 0;//0 - FROM. 1 - TO
    public float SimSpeed =1f;
    public List<GameObject> hopsFound;
    public List<StaticRoute> staticRoutes;
    public List<string> RIPRoutes;


    public void refreshRoutes()
    {
        staticRoutes.Clear();
        RIPRoutes.Clear();

        var ciscoDevices = FindObjectsByType<CiscoDevice>(0);
        foreach(CiscoDevice ciscoDevice in ciscoDevices)
        {
            var staticRoutess = ciscoDevice.staticRoutes;
            foreach(StaticRoute staticRoute in staticRoutess)
            {
                staticRoutes.Add(staticRoute);

            }
            var RIPRoutess = ciscoDevice.RIPNetworks;
            foreach (string RIPRoute in RIPRoutess)
            {
                RIPRoutes.Add(RIPRoute);

            }
        }

    }

    private void Start()
    {
        refreshRoutes();
    }
    private void Awake()
    {
        instance = this;
        refreshList();
    }
    public void refreshList()
    {
        if (instance.DevicesList.Count>0)
        {
            instance.tempApick = instance.DevicesList[instance.A_Dropdown.value];
            instance.tempBpick = instance.DevicesList[instance.B_Dropdown.value];
        }
        instance.A_Dropdown.ClearOptions();
        instance.B_Dropdown.ClearOptions();
        instance.DevicesList.Clear();
        instance.DevicesNames.Clear();
        var DevicesFound = FindObjectsByType<PacketItemPrefabDetails>(0);
        foreach(PacketItemPrefabDetails d in DevicesFound)
        {
            instance.DevicesList.Add(d.gameObject);
            instance.DevicesNames.Add(d.name);
        }
        instance.A_Dropdown.AddOptions(instance.DevicesNames);
        instance.B_Dropdown.AddOptions(instance.DevicesNames);
        instance.A_Dropdown.value = instance.DevicesList.IndexOf(instance.tempApick);
        instance.B_Dropdown.value = instance.DevicesList.IndexOf(instance.tempBpick);
    }

    public void StartPickObject(int what)
    {
        pickingWhat = what;
        isPicking = true;

        if(what == 0)
        {
            APick.GetComponent<Image>().color = UIColorManagerScript.ButtonActiveColor;
            BPick.GetComponent<Image>().color = UIColorManagerScript.ButtonIdleColor;
        }
        else
        {
            BPick.GetComponent<Image>().color = UIColorManagerScript.ButtonActiveColor;
            APick.GetComponent<Image>().color = UIColorManagerScript.ButtonIdleColor;
        }

    }
    public void CancelPick()
    {
        isPicking = false;
        APick.GetComponent<Image>().color = UIColorManagerScript.ButtonIdleColor;
        BPick.GetComponent<Image>().color = UIColorManagerScript.ButtonIdleColor;
    }

    public void pickedObj(GameObject obj)
    {
        CancelPick();
        Debug.Log("Picked object " + obj);
        switch (pickingWhat)
        {
            case 0:
                A_Dropdown.value = DevicesList.IndexOf(obj);
                Debug.Log("Set A dropdown to " + obj);
                break;

            case 1:
                B_Dropdown.value = DevicesList.IndexOf(obj);
                Debug.Log("Set B dropdown to " + obj);
                break;
        }
    }
    public static void refreshDelay()
    {
        instance.Invoke("refreshList", 0.1f);
    }
    public void playSim()
    {
        Debug.Log("doing recursive test: " + DevicesList[A_Dropdown.value]);
        hopsFound.Clear();
        hopsFound.Add(DevicesList[A_Dropdown.value]);
        recursiveTest(DevicesList[A_Dropdown.value]);
    }

    public void recursiveTest(GameObject g)
    {
        Debug.Log("current recursive: " +g);
        var ports = g.GetComponentsInChildren<PortProperties>();
        foreach(var p in ports)
        {

            //List<Node> getChilds = new List<Node>();

            if (p.portHopParent != null && !hopsFound.Contains(p.portHopParent))
            {
                Debug.Log("RECURSIVE FOUND: " + p.portHopParent);
                hopsFound.Add(p.portHopParent);

                //if (p.portHopParent.GetComponent<Node>())
                //{
                //    Debug.Log("getting node of portHopParent " + p.portHopParent);
                //    Node mNode = p.portHopParent.GetComponent<Node>();
                //    Debug.Log("Done! " + mNode);
                //    getChilds.Add(mNode);
                //    g.GetComponent<Node>().children = getChilds.ToArray();
                //    Debug.Log("getting node of portHopParent SUCCESS " + p.portHopParent);
                //}
                //else
                //{
                //    Debug.Log("getting node of portHopParent FAIL " + p.portHopParent);

                //}
                
                
                recursiveTest(p.portHopParent);


            }
            //Debug.Log("setting NODE childrens: " + getChilds);
           
        }
    }

    public void recursiveTest(GameObject g,CMD_Ping ping)
    {
        Debug.Log("current recursive: " + g);
        var ports = g.GetComponentsInChildren<PortProperties>();
        foreach (var p in ports)
        {

            if (p.portHopParent != null && !ping.hops.Contains(p.portHopParent))
            {
                Debug.Log("RECURSIVE FOUND: " + p.portHopParent);
                hopsFound.Add(p.portHopParent);
                ping.hops.Add(p.portHopParent);
                recursiveTest(p.portHopParent, ping);

            }
        }
    }

    public void setAnimSpeed(float speed)
    {
        SimSpeed = speed;
    }
    public CiscoDevice recursiveTestTelnet(GameObject g, string ip)
    {
        Debug.Log("current recursive: " + g);
        var ports = g.GetComponentsInChildren<PortProperties>();
        foreach (var p in ports)
        {
            if(p.portHopParent)
            {
                if( p.address == ip)
                {
                    return p.transform.GetComponentInParent<CiscoDevice>();
                }

            }

            if (p.portHopParent != null && !hopsFound.Contains(p.portHopParent))
            {
                Debug.Log("RECURSIVE FOUND: " + p.portHopParent);
                hopsFound.Add(p.portHopParent);
               return recursiveTestTelnet(p.portHopParent, ip);

            }
        }
        return null;
    }
    public DHCPPool recursiveTestDHCP(GameObject g)
    {
        DHCPPool poolReturn = null;
        List<DHCPPool> pools = null;
        if (g.GetComponent<CiscoDevice>())
        {

            var ciscoPorts = g.GetComponentsInChildren<CiscoEthernetPort>();
            pools = g.GetComponent<CiscoDevice>().DHCPPools;
            foreach (var p in ciscoPorts)
            {
                if (pools != null)
                {
                    foreach (var pool in pools)
                    {
                        if (IsIPInNetwork(p.address,
                            pool.network.Split("/")[0],
                            SubnetDictionary.ConvertCIDRToSubnetMask(int.Parse(pool.network.Split("/")[1]))
                            ))
                        {
                            return pool;
                        }
                    }
                }

            }
            //Debug.Log("Gotten POOLS list from " + g);
            //Debug.Log("POOLS: " + pools);
        }

    

        Debug.Log("current recursive: " + g);
        var ports = g.GetComponentsInChildren<PortProperties>();
        foreach (var p in ports)
        {

            if (p.portHopParent != null && !hopsFound.Contains(p.portHopParent) && p.portHop.PortFunction==PortTypes.Function.EthernetPort)
            {
                if (p.portHop.TryGetComponent<CiscoEthernetPort>(out CiscoEthernetPort cep))
                {
                    Debug.Log("Found CEP " + cep);
                    if (cep.noShut)
                    {
                        Debug.Log("RECURSIVE FOUND: " + p.portHopParent);
                        hopsFound.Add(p.portHopParent);
                        poolReturn = recursiveTestDHCP(p.portHopParent);
                    }

                }
                else
                {
                    Debug.Log("RECURSIVE FOUND: " + p.portHopParent);
                    hopsFound.Add(p.portHopParent);
                    poolReturn = recursiveTestDHCP(p.portHopParent);
                }
        

            }
        }
        return poolReturn;
    }
    public static bool IsIPInNetwork(string ipAddress, string networkAddress, string subnetMask)
    {
        try
        {
            // Convert IP, network address, and subnet mask to byte arrays
            IPAddress ip = IPAddress.Parse(ipAddress);
            IPAddress network = IPAddress.Parse(networkAddress);
            IPAddress mask = IPAddress.Parse(subnetMask);

            byte[] ipBytes = ip.GetAddressBytes();
            byte[] networkBytes = network.GetAddressBytes();
            byte[] maskBytes = mask.GetAddressBytes();

            // Check if IP is in the network range
            for (int i = 0; i < ipBytes.Length; i++)
            {
                if ((ipBytes[i] & maskBytes[i]) != (networkBytes[i] & maskBytes[i]))
                {
                    return false;
                }
            }

            return true;
        }
        catch (Exception e)
        {
            Debug.LogError($"Error: {e.Message}");
            return false;
        }
    }



}
