using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New SetMOTDCommand Command", menuName = "Terminal/SetMOTDCommand Command")]
public class SetMOTDCommand : ConsoleCommand
{
    public override bool Process(string[] args)
    {
        if (args.Length > 1)
        {
            if (args[0] == "motd")
            {
                TerminalConsoleBehavior.instance.MOTD = args[1];
                
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
