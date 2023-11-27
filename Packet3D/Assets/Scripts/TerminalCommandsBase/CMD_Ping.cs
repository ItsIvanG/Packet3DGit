using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "New CMD_Ping Command", menuName = "Terminal/CMD_Ping Command")]
public class CMD_Ping : ConsoleCommand
{
    public override bool Process(string[] args)
    {
        TerminalConsoleBehavior.printToTerminal("PING!");
        return true;
    }
}
