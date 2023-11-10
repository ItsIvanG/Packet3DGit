using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerminalLogin 
{
    public static void checkLogin()
    {
        if(TerminalConsoleBehavior.instance.currentPrivilege == TerminalPrivileges.privileges.loggedOut)
        {
            TerminalConsoleBehavior.printToTerminal(TerminalConsoleBehavior.instance.MOTD+"\n");
            if(TerminalConsoleBehavior.instance.localUsername!="")
            {
                TerminalConsoleBehavior.instance.enteringLocalWhat = 0;
                TerminalConsoleBehavior.printToTerminal("User Access Verification: \n");
            }
            else
            {
                TerminalConsoleBehavior.instance.enteringLocalWhat = 2;
            }
        }
    }
}
