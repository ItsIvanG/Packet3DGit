using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Config Command", menuName = "Terminal/Config Command")]
public class ConfigCommand : ConsoleCommand
{
    
    public override bool Process(string[] args)
    {

        if(args.Length == 1)
        {

            if (args[0] == "terminal")
            {
                TerminalConsoleBehavior.instance.currentPrivilege = TerminalPrivileges.privileges.config;
                return true;
            }
            else
            {
                return false;
            }

        }
        else
        {
            return false;
        }
    }
}
