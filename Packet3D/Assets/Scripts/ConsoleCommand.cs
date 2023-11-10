using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ConsoleCommand : ScriptableObject
{

    public string CommandWord;
    public List<string> CommandArgs;
    public TerminalPrivileges.privileges CommandPrivilege;

    public abstract bool Process(string[] args);

}
