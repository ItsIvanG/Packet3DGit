using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "New NoShutInterface Command", menuName = "Terminal/NoShutInterface Command")]
public class NoShutInterface : ConsoleCommand//
{
    public override bool Process(string[] args)
    {
        if (args[0] == "shutdown")
        {
            CiscoDevice ciscoDevice = TerminalConsoleBehavior.instance.currentObj.GetComponent<CiscoDevice>();
            ciscoDevice.interfacePort.noShut = true;
            return true;
        }
        else
        {
            return false;
        }
    }
}
