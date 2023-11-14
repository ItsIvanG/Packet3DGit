using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New DHCPDnsserver Command", menuName = "Terminal/DHCPDnsserver Command")]
public class DHCPDnsserver : ConsoleCommand
{
    public override bool Process(string[] args)
    {
        CiscoDevice ciscoDevice = TerminalConsoleBehavior.instance.currentObj.GetComponent<CiscoDevice>();

        if (args.Length == 1)
        {
            if (ciscoDevice.interfacePort != null)
            {

                ciscoDevice.interfacePort.dnsserver = args[0];

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
