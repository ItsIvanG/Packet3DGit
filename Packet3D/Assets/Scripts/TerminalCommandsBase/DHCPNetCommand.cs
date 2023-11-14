using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New DHCPNetCommand Command", menuName = "Terminal/DHCPNetCommand Command")]
public class DHCPNetCommand : ConsoleCommand
{
    public override bool Process(string[] args)
    {
        CiscoDevice ciscoDevice = TerminalConsoleBehavior.instance.currentObj.GetComponent<CiscoDevice>();

        if (args.Length == 2)
        {
            if (ciscoDevice.interfacePort != null)
            {

                if (SubnetDictionary.getPrefix(args[1]) != "/?")
                {
                    ciscoDevice.interfacePort.network = args[0];
                    ciscoDevice.interfacePort.networkSubnet = args[1];
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
