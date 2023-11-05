using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DesktopCanvasScript : MonoBehaviour
{
    public static DesktopCanvasScript instance;
    public GameObject currentPC;
    public TextMeshProUGUI PCLabel;
    public TMP_Dropdown ethernetPortsDropdown;
    public List<PortProperties> ethernetPorts = new List<PortProperties>();
    public List<string> ethernetPortNames = new List<string>();
    public Toggle DHCPToggle;
    public Toggle StaticToggle;
    public TMP_InputField IPInput;
    public TMP_InputField SubnetInput;
    public TMP_InputField GatewayInput;
    public TMP_InputField DNSInput;
    public TextMeshProUGUI errorString;
    public GameObject IPPanel;

    private void Awake()
    {
        instance = this;
        instance.gameObject.SetActive(false);
    }

    public static void showDesktopCanvas(GameObject pc)
    {
        instance.ethernetPorts.Clear();
        instance.ethernetPortNames.Clear();
        instance.currentPC = pc;
        instance.gameObject.SetActive(true);
        instance.PCLabel.text = pc.name;
        

        var ports = pc.GetComponentsInChildren<PortProperties>();
        foreach(var p in ports)
        {
            if (p.PortType.ToString().ToLower() == "rj45")
            {
                instance.ethernetPorts.Add(p);
                instance.ethernetPortNames.Add(p.PortName);
            }
            
        }
        instance.ethernetPortsDropdown.ClearOptions();
        instance.ethernetPortsDropdown.AddOptions(instance.ethernetPortNames);

        instance.getIPdetails();
    }
    public void getIPdetails()
    {
        PortProperties pp = ethernetPorts[ethernetPortsDropdown.value];
        Debug.Log("Getting port properties: " + pp);
        if (pp.isStaticIP)
        {
            StaticToggle.isOn = true;
            DHCPToggle.isOn = false;
        }
        else
        {
            StaticToggle.isOn = false;
            DHCPToggle.isOn = true;
        }
        IPInput.text = pp.address;
        SubnetInput.text = pp.subnet;
        GatewayInput.text = pp.defaultgateway;
        DNSInput.text = pp.dnsserver;

    }

    public void setIPdetails()
    {
        PortProperties pp = ethernetPorts[ethernetPortsDropdown.value];
        Debug.Log("SETTING port properties: " + pp);

        if (SubnetDictionary.getPrefix(SubnetInput.text) == "/?")
        {
            errorString.gameObject.SetActive(true);
            errorString.text = "ERROR: Subnet invalid or unsupported.";
        }
        else
        {
            pp.isStaticIP = StaticToggle.isOn;
            pp.address = IPInput.text;
            pp.subnet = SubnetInput.text;
            pp.defaultgateway = GatewayInput.text;
            pp.dnsserver = DNSInput.text;
            IPPanel.gameObject.SetActive(false);
        }

    }
}
