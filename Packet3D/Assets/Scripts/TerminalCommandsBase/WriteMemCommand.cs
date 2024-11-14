using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New WriteMemCommand Command", menuName = "Terminal/WriteMemCommand Command")]
public class WriteMemCommand : ConsoleCommand
{
    public override bool Process(string[] args)
    {
        TerminalConsoleBehavior.instance.currentObj.GetComponent<CiscoDevice>().writeMems.Add(Time.time);
        return true;
    }
}
