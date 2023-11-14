using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New LocalUserPassCommand Command", menuName = "Terminal/LocalUserPassCommand Command")]
public class LocalUserPassCommand : ConsoleCommand
{
    public override bool Process(string[] args)
    {
        if (args.Length > 1 && args.Length<5)
        {
            if (args[1] == "secret")
            {
                TerminalConsoleBehavior.instance.localUsername = args[0];
                TerminalConsoleBehavior.instance.localPassword = args[2];
                return true;
            }
            else//
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
