using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "New SwitchportCommand", menuName = "Terminal/SwitchportCommand Command")]
public class SwitchportCommand : ConsoleCommand
{
    public GameObject vlanPrefab;
    public override bool Process(string[] args)
    {
        CiscoDevice ciscoDevice = TerminalConsoleBehavior.instance.currentObj.GetComponent<CiscoDevice>();
        CiscoEthernetPort ciscoEthernetPort = ciscoDevice.interfacePort;
        List<CiscoEthernetPort> ciscoEthernetPortRange = ciscoDevice.interfaceRange;
        if (args[0] == "mode")
        {
            if (args[1] == "trunk")
            {
                if (TerminalConsoleBehavior.instance.currentConfigLevel == TerminalPrivileges.specificConfig.Interface)
                {
                    ciscoEthernetPort.switchportMode = CiscoEthernetPort.switchportModes.Trunk;
                }
                else if (TerminalConsoleBehavior.instance.currentConfigLevel == TerminalPrivileges.specificConfig.InterfaceRange)
                {
                    foreach(CiscoEthernetPort port in ciscoEthernetPortRange)
                    {
                        port.switchportMode = CiscoEthernetPort.switchportModes.Trunk;
                    }
                }

                    return true;
            } 
            else if (args[1] == "access")
            {
                if (TerminalConsoleBehavior.instance.currentConfigLevel == TerminalPrivileges.specificConfig.Interface)
                {
                    ciscoEthernetPort.switchportMode = CiscoEthernetPort.switchportModes.Access;
                }
                else if (TerminalConsoleBehavior.instance.currentConfigLevel == TerminalPrivileges.specificConfig.InterfaceRange)
                {
                    foreach (CiscoEthernetPort port in ciscoEthernetPortRange)
                    {
                        port.switchportMode = CiscoEthernetPort.switchportModes.Access;
                    }
                }

                return true;
            }
            else
            {

                return false;
            }
        } 
        else if (args[0] == "access" && args[1]=="vlan" && args.Length<4)
        {
            if (TerminalConsoleBehavior.instance.currentConfigLevel == TerminalPrivileges.specificConfig.Interface)
            {
                ciscoEthernetPort.switchportAccessVlan = int.Parse(args[2]);
            }
            else if (TerminalConsoleBehavior.instance.currentConfigLevel == TerminalPrivileges.specificConfig.InterfaceRange)
            {
                foreach (CiscoEthernetPort port in ciscoEthernetPortRange)
                {
                    port.switchportAccessVlan = int.Parse(args[2]);
                }
            }
            Vlan vlanComponent = null;
            Transform vlanTransform = TerminalConsoleBehavior.instance.currentObj.transform.Find("VLAN" + args[2]);
            if (vlanTransform == null)
            {
                TerminalConsoleBehavior.printToTerminal("Access VLAN does not exist. Creating vlan " + args[2]);
                Debug.Log("Creating vlan " + args[2]);
                GameObject vlan = Instantiate(vlanPrefab, TerminalConsoleBehavior.instance.currentObj.transform);
                vlan.name = "VLAN" + args[2];
                vlanComponent = vlan.GetComponent<Vlan>();
                vlanComponent.PortName = "VLAN" + args[2];
                vlanComponent.enabled = true;

                vlanComponent.vlanNumber = int.Parse(args[2]);
                vlanComponent.updateVlanPorts();

            }
            else
            {
                Debug.Log("Vlan exists, accessing.");
                vlanComponent = vlanTransform.GetComponent<Vlan>();
                vlanComponent.updateVlanPorts();
            }
            PropertiesTab.instance.updatePropertiesTab(TerminalConsoleBehavior.instance.currentObj.transform);
            return true;
        }
        else
        {
            return false;
        }

    }
}
