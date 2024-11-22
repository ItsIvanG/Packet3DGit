using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="New LinCon Command", menuName = "Terminal/LinCon Command")]
public class LineConsoleCommand : ConsoleCommand
{
    
    public override bool Process(string[] args)
    {
        var cd = TerminalConsoleBehavior.instance.currentObj.GetComponent<CiscoDevice>();
        if (args[0] == "console")
        {
            foreach(var lcs in cd.lineConsoles)
            {
                if (lcs.index == Int32.Parse(args[1]))
                {
                    cd.currentLineCon = Int32.Parse(args[1]);
                    TerminalConsoleBehavior.instance.currentConfigLevel = TerminalPrivileges.specificConfig.line;
                    cd.lineConfig = TerminalPrivileges.lineConfig.console;
                    return true;
                }

            }
            if (Int32.Parse(args[1]) < cd.linConAvailable)
            {
                LineConsole lc = new LineConsole();
                lc.index = Int32.Parse(args[1]);
                cd.lineConsoles.Add(lc);

                cd.currentLineCon = Int32.Parse(args[1]);
                TerminalConsoleBehavior.instance.currentConfigLevel = TerminalPrivileges.specificConfig.line;
                cd.lineConfig = TerminalPrivileges.lineConfig.console;
                return true;
            }
            else
            {
                TerminalConsoleBehavior.printToTerminal("Console index out of bounds");
                return false;
            }

        }
        else  if (args[0] == "vty" && int.TryParse(args[1],out int start) && int.TryParse(args[2], out int end))
        {
            foreach (var lVTYs in cd.lineVTYs)
            {
                lVTYs.availableVTYconnections = end - start + 1;
                TerminalConsoleBehavior.instance.currentConfigLevel = TerminalPrivileges.specificConfig.line;
                cd.lineConfig = TerminalPrivileges.lineConfig.vty;
                return true;

            }
            LineConsole lc = new LineConsole();
            lc.availableVTYconnections = end-start+1;
            cd.lineVTYs.Add(lc);

            TerminalConsoleBehavior.instance.currentConfigLevel = TerminalPrivileges.specificConfig.line;
            cd.lineConfig = TerminalPrivileges.lineConfig.vty;
            return true;
        }

        else
        {
            return false;
        }
          
    }
}
