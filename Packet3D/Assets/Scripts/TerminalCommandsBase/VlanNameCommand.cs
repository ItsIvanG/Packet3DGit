using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New VlanName Command", menuName = "Terminal/VlanName Command")]

public class VlanName : ConsoleCommand
{
    public override bool Process(string[] args)
    {
        if (args.Length == 1)
        {
            CiscoDevice ciscoDevice = TerminalConsoleBehavior.instance.currentObj.GetComponent<CiscoDevice>();
            ciscoDevice.configVlan.vlanName = args[0];
            PropertiesTab.updatePropertiesTab(TerminalConsoleBehavior.instance.currentObj.transform);
            return true;
        }
        else
        {
            return false;
        }
    }
}
