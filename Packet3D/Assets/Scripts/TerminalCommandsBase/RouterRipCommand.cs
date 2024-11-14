using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "New RouterRip Command", menuName = "Terminal/RouterRip Command")]
public class RouterRipCommand : ConsoleCommand
{
    public override bool Process(string[] args)
    {
        var ciscoDevice = TerminalConsoleBehavior.instance.currentObj.GetComponent<CiscoDevice>();
        if (args[0] == "rip")
        {
            //ciscoDevice.currentConfigLevel = TerminalPrivileges.specificConfig.router;
            TerminalConsoleBehavior.instance.currentConfigLevel = TerminalPrivileges.specificConfig.router;
            return true;
        }
        else
        {
            return false;
        }
    }

    
}
