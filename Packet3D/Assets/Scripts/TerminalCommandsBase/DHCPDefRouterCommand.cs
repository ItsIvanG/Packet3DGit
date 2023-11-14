using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New DHCPDefaultRouter Command", menuName = "Terminal/DHCPDefaultRouter Command")]
public class DHCPDefaultRouter : ConsoleCommand
{
    public override bool Process(string[] args)
    {
        CiscoDevice ciscoDevice = TerminalConsoleBehavior.instance.currentObj.GetComponent<CiscoDevice>();

        if (args.Length == 1)
        {
            if (ciscoDevice.interfacePort != null)
            {

                ciscoDevice.interfacePort.defaultRouter = args[0];

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
