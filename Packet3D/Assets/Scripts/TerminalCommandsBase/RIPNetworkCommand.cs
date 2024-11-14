using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "New RIPNetworkCommand Command", menuName = "Terminal/RIPNetworkCommand Command")]
public class RIPNetworkCommand : ConsoleCommand
{
    public override bool Process(string[] args)
    {
        var ciscoDevice = TerminalConsoleBehavior.instance.currentObj.GetComponent<CiscoDevice>();
        if (SubnetDictionary.IsValidIPAddress(args[0]))
        {
            ciscoDevice.RIPNetworks.Add(args[0]);
            return true;
        }
        else
        {
            TerminalConsoleBehavior.printToTerminal("Invalid network.");
            return false;
        }
    }

    
}
