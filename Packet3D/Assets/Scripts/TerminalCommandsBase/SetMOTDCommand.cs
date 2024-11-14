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
                for(int i =1;i<args.Length;i++)
                {

                    if (i > 1) TerminalConsoleBehavior.instance.MOTD += " " + args[i];
                    else TerminalConsoleBehavior.instance.MOTD += args[i];
                }
                
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
