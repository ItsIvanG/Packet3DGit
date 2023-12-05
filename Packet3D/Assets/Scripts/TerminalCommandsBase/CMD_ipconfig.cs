using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "New CMD_ipconfig Command", menuName = "Terminal/CMD_ipconfig Command")]
public class CMD_ipconfig : ConsoleCommand
{
    public override bool Process(string[] args)
    {
        var PCethernetPorts = TerminalConsoleBehavior.instance.currentObj.GetComponentsInChildren<PCEthernetProperties>();
        foreach(var PCethernetPort in PCethernetPorts)
        {
            TerminalConsoleBehavior.printToTerminal(PCethernetPort.PortName+":");
            TerminalConsoleBehavior.printToTerminal("   IPv4 Address:. . . . . . "+PCethernetPort.address);
            TerminalConsoleBehavior.printToTerminal("   Subnet Mask: . . . . . . " + PCethernetPort.subnet);
            TerminalConsoleBehavior.printToTerminal("   Default Gateway: . . . . " + PCethernetPort.defaultgateway);
        }
        return true;
    }
}
