using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New LinConLoginLocal Command", menuName = "Terminal/LinConLoginLocal Command")]
public class LinConLoginLocal : ConsoleCommand
{
    public override bool Process(string[] args)
    {
        var cd = TerminalConsoleBehavior.instance.currentObj.GetComponent<CiscoDevice>();


        if (args[0] == "local")
        {
            cd.lineConsoles[cd.currentLineCon].usingLocal = true;
            return true;
        }
        else
        {
            return false;
        }
    }
}