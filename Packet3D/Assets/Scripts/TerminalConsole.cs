using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TerminalConsole 
{
    private readonly IEnumerable<ConsoleCommand> commands;
    private bool found=false;
    public TerminalConsole(IEnumerable<ConsoleCommand> commands)
    {
        this.commands = commands;
    }

    public void ProcessCommand(string inputValue, bool flagIfInvalid)
    {
        Debug.Log("PROCESSING COMMAND: "+inputValue);
        string[] inputSplit = inputValue.Split(' ');
        string commandInput = inputSplit[0];

        string[] args = inputSplit.Skip(1).ToArray();

        ProcessCommand(commandInput, args, flagIfInvalid);
    }

    public void ProcessCommand(string commandInput, string[] args, bool flagIfInvalid)
    {
        foreach(var command in commands)
        {
            if ((TerminalConsoleBehavior.instance.currentPrivilege == command.CommandPrivilege && 
                command.specificConfig == TerminalPrivileges.specificConfig.global && 
                TerminalConsoleBehavior.instance.currentConfigLevel == TerminalPrivileges.specificConfig.global)
                || 
                command.CommandPrivilege == TerminalPrivileges.privileges.all 
                ||
                (TerminalConsoleBehavior.instance.currentPrivilege == command.CommandPrivilege &&
                command.specificConfig == TerminalConsoleBehavior.instance.currentConfigLevel)
                ||
                (TerminalConsoleBehavior.instance.currentPrivilege == TerminalPrivileges.privileges.cmd &&
                command.CommandPrivilege == TerminalPrivileges.privileges.cmd)
                )
            {
                
                if (!commandInput.Equals(command.CommandWord))
                {
                    continue;
                }
                if (command.Process(args))
                {

                    if (TutorialScript.instance) TutorialScript.instance.checkWait();
                    if (ActivityScript.instance) ActivityScript.instance.checkWait();
                    found = true;
                    Debug.Log("executing " + command.CommandWord);
                    return;
                }
                else
                {
                    found = true;
                    TerminalConsoleBehavior.printToTerminal("Invalid arguments to command: " + commandInput);
                    return;
                }
                
            }
            else
            {
                found = false;
                continue;
            }
            
        }
        if (!found && flagIfInvalid)
        {
            TerminalConsoleBehavior.printToTerminal("Invalid command: " + commandInput);
        }
    }




}
