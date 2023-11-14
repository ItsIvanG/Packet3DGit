using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New DHCPExclude Command", menuName = "Terminal/DHCPExclude Command")]
public class DHCPExclude : ConsoleCommand
{
    public override bool Process(string[] args)
    {
        CiscoDevice ciscoDevice = TerminalConsoleBehavior.instance.currentObj.GetComponent<CiscoDevice>();

        if (args.Length == 4 && args[0].ToLower() == "dhcp" && args[1].ToLower() == "excluded-address")
        {
            if (ciscoDevice.interfacePort != null)
            {

                ciscoDevice.interfacePort.excludeStart = args[2];
                ciscoDevice.interfacePort.excludeEnd = args[3];
                TerminalConsoleBehavior.instance.currentConfigLevel = TerminalPrivileges.specificConfig.global;

                return true;
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
