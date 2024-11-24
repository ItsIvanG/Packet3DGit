using System.Collections;
using System.Collections.Generic;

using UnityEngine;

[CreateAssetMenu(fileName = "New DHCPNetCommand Command", menuName = "Terminal/DHCPNetCommand Command")]
public class DHCPNetCommand : ConsoleCommand
{
    public override bool Process(string[] args)
    {
        int poolIndex = 0;
        CiscoDevice ciscoDevice = TerminalConsoleBehavior.instance.currentObj.GetComponent<CiscoDevice>();

        if (args.Length == 2)
        {

            for (int i = 0; i < ciscoDevice.DHCPPools.Count; i++)
            {
                if (ciscoDevice.DHCPPools[i].DHCPName == ciscoDevice.currentPool)
                {
                    Debug.Log("Found pool index " + i + "|| NAME: " + ciscoDevice.DHCPPools[i].DHCPName + " || CURRPOOL: " + ciscoDevice.currentPool);

                    poolIndex = i;
                }
                
            }
            Debug.Log("CURRENTPOOL INDEX: " + poolIndex);

            if (SubnetDictionary.getPrefix(args[1]) != "/?")
            {
                ciscoDevice.DHCPPools[poolIndex].network = args[0] + SubnetDictionary.getPrefix(args[1]);

                var ports = ciscoDevice.GetComponentsInChildren<CiscoEthernetPort>();
                foreach (var port in ports)
                {
                    Debug.Log("Testing if IP " + port.address + " is in " + args[0] + " with subnet " + args[1]);
                    try
                    {
                        if (SimulationBehavior.IsIPInNetwork(port.address, args[0], args[1]))
                        {
                            ciscoDevice.DHCPPools[poolIndex].existingIPs.Add(port.address);
                        }
                    } catch
                    {
                        continue;
                    }

                }
                ciscoDevice.DHCPPools[poolIndex].existingIPs.Add(SubnetDictionary.GetNetworkAddress(args[0], args[1]));
                return true;
            }
            else
            {
                TerminalConsoleBehavior.printToTerminal("Invalid subnet");
                return false;
            }
             
             
            //if (ciscoDevice.interfacePort != null)
            //{

            //    if (SubnetDictionary.getPrefix(args[1]) != "/?")
            //    {
            //        ciscoDevice.interfacePort.network = args[0];
            //        ciscoDevice.interfacePort.networkSubnet = args[1];
            //        return true;
            //    }
            //    else
            //    {
            //        TerminalConsoleBehavior.printToTerminal("Invalid subnet");
            //        return false;
            //    }

            //}
            //else
            //{
            //    TerminalConsoleBehavior.printToTerminal("No port interface");
            //    return false;
            //}
        }
        else
        {
            return false;
        }
    }
}
