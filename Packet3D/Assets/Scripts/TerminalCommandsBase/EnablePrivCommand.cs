using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="New EnablePriv Command", menuName = "Terminal/EnablePriv Command")]
public class EnablePrivCommand : ConsoleCommand
{
    public override bool Process(string[] args)
    {

            if (TerminalConsoleBehavior.instance.enablePassword == "")
            {
                TerminalConsoleBehavior.instance.currentPrivilege = TerminalPrivileges.privileges.privileged;
            }
            else
            {
                TerminalConsoleBehavior.instance.authenticatingEnable = true;
                TerminalConsoleBehavior.instance.enteringLocalWhat = 1;
                TerminalConsoleBehavior.instance.currentPrivilege = TerminalPrivileges.privileges.loggedOut;

            }

            return true;
          
    }
}
