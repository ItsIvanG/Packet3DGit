using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New IPDHCPPoolCommand Command", menuName = "Terminal/IPDHCPPoolCommand Command")]
public class IPDHCPPoolAndRouteCommand : ConsoleCommand
{
    public override bool Process(string[] args)
    {
        CiscoDevice ciscoDevice = TerminalConsoleBehavior.instance.currentObj.GetComponent<CiscoDevice>();

        if (args.Length == 3 && args[0].ToLower() == "dhcp" && args[1].ToLower()=="pool")
        {
            CiscoEthernetPort interfacePort = null;
            
            var getAllCiscoPorts = TerminalConsoleBehavior.instance.currentObj.GetComponentsInChildren<CiscoEthernetPort>();

            foreach (CiscoEthernetPort port in getAllCiscoPorts)
            {
                if (port.name == args[2].ToUpper())
                {
                    interfacePort = port;
                }
            }

            if (interfacePort != null)
            {
                Debug.Log("Found port " + args[2]);
                TerminalConsoleBehavior.instance.currentObj.GetComponent<CiscoDevice>().interfacePort = interfacePort;
                TerminalConsoleBehavior.instance.currentConfigLevel = TerminalPrivileges.specificConfig.dhcppool;

                return true;
            }
            
            else
            {
                Debug.Log("Did not find port " + args[2]);
                TerminalConsoleBehavior.printToTerminal("Could not find port:" + args[2] + ".");
                return false;
            }

        }
        else if (args.Length == 4 && args[0].ToLower() == "route")
        {
            
            if (SubnetDictionary.getPrefix(args[2])!="/?")
            {
                TerminalConsoleBehavior.instance.currentObj.GetComponent<RouterBehavior>().routeAddresses.Add(args[1]);
                TerminalConsoleBehavior.instance.currentObj.GetComponent<RouterBehavior>().routeSubnets.Add(args[2]);
                TerminalConsoleBehavior.instance.currentObj.GetComponent<RouterBehavior>().routeDestinations.Add(args[3]);
            }
            else
            {
                TerminalConsoleBehavior.printToTerminal("Invalid subnet");
                return false;
            }
            return true;

        }
        else
        {
            return false;
        }
    }
}
