using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New DHCPExclude Command", menuName = "Terminal/DHCPExclude Command")]
public class DHCPExclude : ConsoleCommand
{
    public override bool Process(string[] args)
    {
        CiscoDevice ciscoDevice = TerminalConsoleBehavior.instance.currentObj.GetComponent<CiscoDevice>();
        List<DHCPExcludes> excs = ciscoDevice.DHCPExcludes;

        if (args.Length == 4 && args[0].ToLower() == "dhcp" && args[1].ToLower() == "excluded-address")
        {
            if (SubnetDictionary.IsValidIPAddress(args[2]) &&
                SubnetDictionary.IsValidIPAddress(args[3]))
            {           
                DHCPExcludes newExc = new DHCPExcludes();
                newExc.excludeBegin = args[2];
                newExc.excludeEnd = args[3];
                excs.Add(newExc);
                return true;
            }
            else
            {
                TerminalConsoleBehavior.printToTerminal("Invalid IPs.");
                return false;
            }


       
         
        }
        else
        {
            return false;
        }
    }
}
