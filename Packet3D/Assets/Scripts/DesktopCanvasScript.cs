using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Microsoft.MixedReality.Toolkit.Experimental.UI;

public class DesktopCanvasScript : MonoBehaviour
{
    public static DesktopCanvasScript instance;
    public GameObject currentPC;
    public TextMeshProUGUI PCLabel;
    public TMP_Dropdown ethernetPortsDropdown;
    public TMP_Dropdown USBPortsDropdown;
    public List<PCEthernetProperties> ethernetPorts;
    public List<string> ethernetPortNames = new List<string>();
    public List<PortProperties> USBPorts = new List<PortProperties>();
    public List<string> USBPortNames = new List<string>();
    public Toggle DHCPToggle;
    public Toggle StaticToggle;
    public TMP_InputField IPInput;
    public TMP_InputField SubnetInput;
    public TMP_InputField GatewayInput;
    public TMP_InputField DNSInput;
    public TextMeshProUGUI errorString;
    public GameObject IPPanel;
    public GameObject cursor;
    public GameObject rightControl;
    public float cursorOffset;
    public NonNativeKeyboard Keyboard;

    private void Awake()
    {
        instance = this;
        instance.gameObject.SetActive(false);
    }

    private void Update()
    {
        RaycastHit hit;
        Physics.Raycast(rightControl.transform.position, rightControl.transform.forward, out hit, 100);
        cursor.transform.position = hit.point-(transform.parent.forward*cursorOffset);
    }

    public static void showDesktopCanvas(GameObject pc)
    {

        ///put screen & keyboard to monitor
        Transform screen = pc.transform.Find("Screen");
        Transform kb = pc.transform.Find("KB");
        instance.transform.parent.position = screen.position;
        instance.transform.parent.rotation = screen.rotation;
        instance.Keyboard.transform.position = kb.position;
        instance.Keyboard.transform.rotation = kb.rotation;
        instance.Keyboard.PresentKeyboard();

        instance.ethernetPorts.Clear();
        instance.ethernetPortNames.Clear();
        instance.USBPorts.Clear();
        instance.USBPortNames.Clear();
        instance.currentPC = pc;
        instance.gameObject.SetActive(true);
        instance.PCLabel.text = pc.name;
        

        var ports = pc.GetComponentsInChildren<PortProperties>();
        var EthPorts = pc.GetComponentsInChildren<PCEthernetProperties>();
        foreach (var p in ports)
        {
            if (p.PortType == PortTypes.Type.USB)
            {
                instance.USBPorts.Add(p);
                instance.USBPortNames.Add(p.PortName);
            }
        }
        foreach(var p in EthPorts)
        {
           
            instance.ethernetPorts.Add(p);
            instance.ethernetPortNames.Add(p.PortName);
            
        }
        instance.ethernetPortsDropdown.ClearOptions();
        instance.ethernetPortsDropdown.AddOptions(instance.ethernetPortNames);
        instance.USBPortsDropdown.ClearOptions();
        instance.USBPortsDropdown.AddOptions(instance.USBPortNames);
        instance.getIPdetails();
        instance.setDHCPStaticToggles();
    }
    public void getIPdetails()
    {
        PCEthernetProperties pp = ethernetPorts[ethernetPortsDropdown.value];
        Debug.Log("Getting port properties: " + pp);
       
        IPInput.text = pp.address;
        SubnetInput.text = pp.subnet;
        GatewayInput.text = pp.defaultgateway;
        DNSInput.text = pp.dnsserver;

    }

    public void setIPdetails()
    {
        PCEthernetProperties pp = ethernetPorts[ethernetPortsDropdown.value];
        Debug.Log("SETTING port properties: " + pp);

        if (SubnetDictionary.getPrefix(SubnetInput.text) == "/?")
        {
            PopupMessage.showMessage("Error", "Subnet invalid or unsupported.", PopupMessage.MsgType.Error);
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
    public void setPCtoDHCP()
    {
        PCEthernetProperties pp = ethernetPorts[ethernetPortsDropdown.value];

        

        CiscoEthernetPort ciscoPortHop = null;
        //if (pp.portHopParent.GetComponentsInChildren<CiscoEthernetPort>())
        //{
        //    PopupMessage.showMessage("Error", "No valid connection on ethernet port.", PopupMessage.MsgType.Error);
        //    return;
        //}
        var getAllCiscoPorts = pp.portHopParent.GetComponentsInChildren<CiscoEthernetPort>();
        
        foreach (CiscoEthernetPort port in getAllCiscoPorts)
        {
            if (port.name == pp.portHop.name)
            {
                ciscoPortHop = port;
            }
        }

        pp.isStaticIP = false;
        Debug.Log("attempting dhcp address excludeEnd " + ciscoPortHop.excludeEnd);
        var excludeEndSplit = ciscoPortHop.excludeEnd.Split(".");
        pp.address = excludeEndSplit[0] + "." + excludeEndSplit[1] + "." + excludeEndSplit[2] + "." + (int.Parse(excludeEndSplit[3])+1);
        pp.subnet = ciscoPortHop.networkSubnet;
        pp.dnsserver = ciscoPortHop.dnsserver;
        pp.defaultgateway = ciscoPortHop.defaultRouter;

        getIPdetails();
    }
    public void setPCstaticIP()
    {
        PCEthernetProperties pp = ethernetPorts[ethernetPortsDropdown.value];
        pp.isStaticIP = true;
        //getIPdetails();
    }
    public void showPCTerminal()
    {
        if (USBPorts[USBPortsDropdown.value].portHopParent && USBPorts[USBPortsDropdown.value].portHop.PortFunction==PortTypes.Function.Console)
        {
            TerminalCanvasScript.ShowTerminal(USBPorts[USBPortsDropdown.value].portHopParent);
        }
        else
        {
            Debug.Log("NO Console Device plugged in USB port!");
            PopupMessage.showMessage("Error", "NO Console Device plugged in USB port!", PopupMessage.MsgType.Error);
        }
    }
    public void setDHCPStaticToggles()
    {
        PCEthernetProperties pp = instance.ethernetPorts[instance.ethernetPortsDropdown.value];
        if (pp.isStaticIP)
        {
            instance.StaticToggle.isOn = true;
            instance.DHCPToggle.isOn = false;
        }
        else
        {
            instance.StaticToggle.isOn = false;
            instance.DHCPToggle.isOn = true;
        }
    }
}
