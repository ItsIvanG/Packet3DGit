using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New DHCPDnsserver Command", menuName = "Terminal/DHCPDnsserver Command")]
public class DHCPDnsserver : ConsoleCommand
{
    public override bool Process(string[] args)
    {
        int poolIndex = 0;
        CiscoDevice ciscoDevice = TerminalConsoleBehavior.instance.currentObj.GetComponent<CiscoDevice>();

        if (args.Length == 1)
        {

            for (int i = 0; i < ciscoDevice.DHCPPools.Count; i++)
            {
                if (ciscoDevice.DHCPPools[i].Name == ciscoDevice.currentPool) poolIndex = i;
                break;
            }

            ciscoDevice.DHCPPools[poolIndex].defaultDNS = args[0];



            return true;


        }
        else
        {
            return false;
        }
    }
}
