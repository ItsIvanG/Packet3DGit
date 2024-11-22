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
    //public GameObject cursor;
    //public GameObject rightControl;
    //public float cursorOffset;
    public NonNativeKeyboard Keyboard;
    public TextMeshProUGUI COMString;
    public TMP_InputField COMInput;
    public Toggle SerialToggle;
    //public PCBehavior.CurrentMenu currentMenu = PCBehavior.CurrentMenu.Desktop;

    private void Awake()
    {
        instance = this;
        instance.gameObject.SetActive(false);
    }

    //private void Update()
    //{
    //    RaycastHit hit;
    //    Physics.Raycast(rightControl.transform.position, rightControl.transform.forward, out hit, 100);
    //    cursor.transform.position = hit.point-(transform.parent.forward*cursorOffset);
    //}

    public static void showDesktopCanvas(GameObject pc)
    {

        ///put screen & keyboard to monitor
        Transform screen = pc.transform.Find("Screen");
        Transform kb = pc.transform.Find("KB");
        instance.transform.parent.position = screen.position;
        instance.transform.parent.rotation = screen.rotation;

        if (ActivityScript.instance)
        {
            if (!ActivityScript.instance.isDone)
            {
                instance.Keyboard.transform.position = kb.position;
                instance.Keyboard.transform.rotation = kb.rotation;
                instance.Keyboard.PresentKeyboard();
            }
        }
        else
        {
            instance.Keyboard.transform.position = kb.position;
            instance.Keyboard.transform.rotation = kb.rotation;
            instance.Keyboard.PresentKeyboard();
        }

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
        //instance.USBPortsDropdown.ClearOptions();
        //instance.USBPortsDropdown.AddOptions(instance.USBPortNames);
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
        StaticToggle.isOn = pp.isStaticIP;

    }

    public void setIPdetails()
    {
        PCEthernetProperties pp = ethernetPorts[ethernetPortsDropdown.value];
        Debug.Log("SETTING port properties: " + pp);
        if (SubnetDictionary.IsValidIPAddress(GatewayInput.text))
        {
            if (SubnetDictionary.IsValidIPAddress(DNSInput.text))
            {
                if (SubnetDictionary.IsValidIPAddress(IPInput.text))
                {
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
                else
                {
                    PopupMessage.showMessage("Error", "Invalid IP address.", PopupMessage.MsgType.Error);
                }

            }
            else
            {
                PopupMessage.showMessage("Error", "Invalid DNS address.", PopupMessage.MsgType.Error);

            }
        }
        else
        {
            PopupMessage.showMessage("Error", "Invalid gateway address.", PopupMessage.MsgType.Error);

        }
       

    }
    public DHCPPool lastPool = null;
    public void setPCtoDHCP()
    {
        if (DHCPToggle.isOn) {
            PCEthernetProperties pp = ethernetPorts[ethernetPortsDropdown.value];
            CiscoEthernetPort ciscoPortHop = null;
            pp.isStaticIP = false;

            SimulationBehavior.instance.hopsFound.Clear();
            DHCPPool pool = SimulationBehavior.instance.recursiveTestDHCP(currentPC);

            if (pool != null)
            {
                lastPool = pool;
                pp.address = pool.GetNextAvailableIP();
                pool.existingIPs.Add(pp.address);
                pp.subnet = SubnetDictionary.ConvertCIDRToSubnetMask(int.Parse(pool.network.Split("/")[1]));
                pp.dnsserver = pool.defaultDNS;
                pp.defaultgateway = pool.defaultGateway;
            }
            else
            {
                PopupMessage.showMessage("DHCP Fail", "APIPA is being used.", PopupMessage.MsgType.Error);
                pp.address = "169.254." + Random.Range(0, 255) + "." + Random.Range(0, 255);
                pp.subnet = "255.255.0.0";
                pp.dnsserver = "0.0.0.0";
                pp.defaultgateway = "0.0.0.0";
            }

        }



        //if (pp.portHopParent.GetComponentsInChildren<CiscoEthernetPort>())
        //{
        //    PopupMessage.showMessage("Error", "No valid connection on ethernet port.", PopupMessage.MsgType.Error);
        //    return;
        //}
        //var getAllCiscoPorts = pp.portHopParent.GetComponentsInChildren<CiscoEthernetPort>();

        //foreach (CiscoEthernetPort port in getAllCiscoPorts)
        //{
        //    if (port.name == pp.portHop.name)
        //    {
        //        ciscoPortHop = port;
        //    }
        //}

        //TODO DHCP

        Debug.Log("attempting dhcp address excludeEnd ");
        //var excludeEndSplit = ciscoPortHop.excludeEnd.Split(".");
        ////pp.address = excludeEndSplit[0] + "." + excludeEndSplit[1] + "." + excludeEndSplit[2] + "." + (int.Parse(excludeEndSplit[3])+1);
        //pp.subnet = ciscoPortHop.networkSubnet;
        //pp.dnsserver = ciscoPortHop.dnsserver;
        //pp.defaultgateway = ciscoPortHop.defaultRouter;


        getIPdetails();
    }
    public void setPCstaticIP()
    {
        if (StaticToggle.isOn)
        {
    
            PCEthernetProperties pp = ethernetPorts[ethernetPortsDropdown.value];
            if(lastPool!=null) lastPool.existingIPs.Remove(pp.address);
            pp.isStaticIP = true;
        }

        //getIPdetails();
    }
    public void showPCTerminal()
    {
        if (SerialToggle.isOn)
        {
            bool found = false;
            for (int i = 0; i < USBPorts.Count; i++)
            {

                if (USBPortNames[i] == COMInput.text && USBPorts[i].portHop.PortFunction == PortTypes.Function.Console)
                {
                    TerminalCanvasScript.ShowTerminal(USBPorts[i].portHopParent);
                    currentPC.GetComponent<PCBehavior>().currentMenu = PCBehavior.CurrentMenu.Terminal;
                    found = true;
                }

            }
            if (!found)
            {
                Debug.Log("NO Console Device plugged in USB port!");
                PopupMessage.showMessage("Error", "Serial not found", PopupMessage.MsgType.Error);
            }
        }
        else
        {
            if (COMInput.text != "")
            {
                SimulationBehavior.instance.hopsFound.Clear();

                CiscoDevice cd = SimulationBehavior.instance.recursiveTestTelnet(currentPC, COMInput.text);
                if (cd)
                {
                    if (cd.checkLoginLocalVTY())
                    {
                        cd.currentPrivilege = TerminalPrivileges.privileges.loggedOut;
                        TerminalConsoleBehavior.instance.currentPrivilege = TerminalPrivileges.privileges.loggedOut;
                        TerminalCanvasScript.ShowTerminal(cd.gameObject);
                        currentPC.GetComponent<PCBehavior>().successTelnet = COMInput.text;
                    }
                    else
                    {
                        PopupMessage.showMessage("Error", "Connection closed by host.", PopupMessage.MsgType.Error);

                    }
                }
                else
                {
                    PopupMessage.showMessage("Error", "IP address not found", PopupMessage.MsgType.Error);
                }
            }
           
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

    public void saveDesktopState(int state)
    {
        currentPC.GetComponent<PCBehavior>().currentMenu = (PCBehavior.CurrentMenu)state;

    }
    public void getDesktopState()
    {

    }
    public void UpdateCOMList()
    {
        COMString.text = "";
        for (int i = 0; i < USBPorts.Count; i++)
        {
            if (USBPorts[i].portHop != null)
            {
                COMString.text += USBPortNames[i] + "\n";
            }
        }
    }
}
