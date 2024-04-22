using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New IPAddCommand Command", menuName = "Terminal/IPAddCommand Command")]
public class IPAddCommand : ConsoleCommand
{
    public override bool Process(string[] args)
    {
        CiscoDevice ciscoDevice = TerminalConsoleBehavior.instance.currentObj.GetComponent<CiscoDevice>();

        if (args.Length == 3)
        {
            if(ciscoDevice.interfacePort != null)
            {

                if (args[0] == "address")
                {
                    if (SubnetDictionary.getPrefix(args[2]) != "/?")
                    {
                        ciscoDevice.interfacePort.address = args[1];
                        ciscoDevice.interfacePort.subnet = args[2];
                        PropertiesTab.instance.updatePropertiesTab(PropertiesTab.instance.currentObj);
                        return true;
                    }
                    else
                    {
                        TerminalConsoleBehavior.printToTerminal("Invalid subnet");
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }
            else
            {
                TerminalConsoleBehavior.printToTerminal("No port interface");
                return false;
            }
        }
        else
        {
            return false;
        }
    }
}
