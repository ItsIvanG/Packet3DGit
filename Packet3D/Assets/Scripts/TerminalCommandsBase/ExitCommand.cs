using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "New Exit Command", menuName = "Terminal/Exit Command")]

public class ExitCommand : ConsoleCommand
{
    public override bool Process(string[] args)
    {
        Debug.Log("EXIT");
        if(args.Length == 0)
        {
            var cd = TerminalConsoleBehavior.instance.currentObj.GetComponent<CiscoDevice>();
            if (TerminalConsoleBehavior.instance.currentPrivilege < TerminalPrivileges.privileges.config)
            {

                TerminalConsoleBehavior.instance.currentPrivilege -= 1;
                cd.currentPrivilege = TerminalConsoleBehavior.instance.currentPrivilege;

                TerminalLogin.checkLogin();
            } 
            else if(TerminalConsoleBehavior.instance.currentPrivilege == TerminalPrivileges.privileges.config)
            {
                if ((int)TerminalConsoleBehavior.instance.currentConfigLevel > 0)
                {
                    TerminalConsoleBehavior.instance.currentConfigLevel = 0;
                    TerminalConsoleBehavior.instance.currentObj.GetComponent<CiscoDevice>().interfacePort = null;
                    TerminalConsoleBehavior.instance.currentObj.GetComponent<CiscoDevice>().configVlan = null;
                    TerminalConsoleBehavior.instance.currentObj.GetComponent<CiscoDevice>().interfaceRange.Clear();
                }
                else
                {
                    TerminalConsoleBehavior.instance.currentPrivilege -= 1;
                }
            }
            return true;
        }
        else
        {
            return false;
        }
    }
}
