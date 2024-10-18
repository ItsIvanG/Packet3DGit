using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New EnableSecretPass Command", menuName = "Terminal/EnableSecretPass Command")]
public class EnableSecretPassCommand :ConsoleCommand
{
    public override bool Process(string[] args)
    {
        if(args.Length > 1)
        {
            if (args[0] == "secret")
            {
                TerminalConsoleBehavior.instance.enablePassword = args[1];
                //REMEMBER TO ADD TO ROUTER/SWITCH ITSELF!!!
                TerminalConsoleBehavior.instance.currentObj.GetComponent<CiscoDevice>().enablePassword = args[1];
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
