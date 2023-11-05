using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TerminalConsole 
{
    private readonly IEnumerable<IConsoleCommand> commands;
    public TerminalConsole(IEnumerable<IConsoleCommand> commands)
    {
        this.commands = commands;
    }

    public void ProcessCommand(string inputValue)
    {
        Debug.Log("PROCESSING COMMAND: "+inputValue);
        string[] inputSplit = inputValue.Split(' ');
        string commandInput = inputSplit[0];

        string[] args = inputSplit.Skip(1).ToArray();

        ProcessCommand(commandInput, args);
    }

    public void ProcessCommand(string commandInput, string[] args)
    {
        foreach(var command in commands)
        {
            if(!commandInput.Equals(command.CommandWord, StringComparison.OrdinalIgnoreCase))
            {
                continue;
            }

            if (command.Process(args))
            {
                return;
            }
        }
    }




}
