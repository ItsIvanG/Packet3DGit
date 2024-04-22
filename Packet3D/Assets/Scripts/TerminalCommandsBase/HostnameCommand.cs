using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="New Hostname Command", menuName = "Terminal/Hostname Command")]
public class HostnameCommand : ConsoleCommand
{
    public override bool Process(string[] args)
    {


        if (args.Length == 1)
        {

            Debug.Log("Setting hostname to: " + args[0]);

            TerminalConsoleBehavior.instance.currentObj.GetComponent<CiscoDevice>().hostname = args[0];

            PropertiesTab.instance.updatePropertiesTab(TerminalConsoleBehavior.instance.currentObj.transform);

            return true;
        }
        else
        {
            Debug.Log("hostname args invalid");
            return false;
            
        }
    }
}

