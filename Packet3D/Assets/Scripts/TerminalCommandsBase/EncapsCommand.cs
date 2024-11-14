using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "New EncapsCommand Command", menuName = "Terminal/EncapsCommand Command")]

public class EncapsCommand : ConsoleCommand
{
    //public GameObject SubportPrefab;
    public override bool Process(string[] args)
    {
        CiscoDevice ciscoDevice = TerminalConsoleBehavior.instance.currentObj.GetComponent<CiscoDevice>();

        if ( args[0]=="dot1q")
        {
            if (args.Length == 2 && int.TryParse(args[1],out int vlan)){

                Subport subport = (Subport) ciscoDevice.interfacePort;
                subport.encapsulationVlan = vlan;
                subport.isEnscapsulated = true;
            }
            return true;
        }
        else
        {
           
            return false;
        }
    }
}
