using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IConsoleCommand 
{
    string CommandWord { get; }
    TerminalPrivileges.privileges CommandPrivilege { get; }
    bool Process(string[] args);
}
