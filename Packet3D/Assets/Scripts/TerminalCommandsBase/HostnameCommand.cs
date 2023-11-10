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

            if (TerminalConsoleBehavior.instance.currentObj.GetComponent<RouterProperties>())
            {
                TerminalConsoleBehavior.instance.currentObj.GetComponent<RouterProperties>().hostname = args[0];
            }
            else if (TerminalConsoleBehavior.instance.currentObj.GetComponent<SwitchProperties>())
            {
                TerminalConsoleBehavior.instance.currentObj.GetComponent<SwitchProperties>().hostname = args[0];
            }
            PropertiesTab.updatePropertiesTab(TerminalConsoleBehavior.instance.currentObj.transform);

            return true;
        }
        else
        {
            Debug.Log("hostname args invalid");
            return false;
            
        }
    }
}

