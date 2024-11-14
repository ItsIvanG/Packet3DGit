using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New IPDHCPPoolCommand Command", menuName = "Terminal/IPDHCPPoolCommand Command")]
public class IPDHCPPoolAndRouteCommand : ConsoleCommand
{
    public override bool Process(string[] args)
    {
        CiscoDevice ciscoDevice = TerminalConsoleBehavior.instance.currentObj.GetComponent<CiscoDevice>();

        if (args.Length == 3 && args[0].ToLower() == "dhcp" && args[1].ToLower() == "pool")
        {
            List<DHCPPool> pools = ciscoDevice.DHCPPools;

            bool poolExists=false;

            foreach(var pl in pools)
            {
                if(pl.Name == args[2])
                {
                    poolExists = true;
                    break;
                }
            }

            if (!poolExists)
            {
                DHCPPool pool = new DHCPPool();
                pool.Name = args[2];
                ciscoDevice.DHCPPools.Add(pool);
            }



            ciscoDevice.currentConfigLevel = TerminalPrivileges.specificConfig.dhcppool;
            TerminalConsoleBehavior.instance.currentConfigLevel = TerminalPrivileges.specificConfig.dhcppool;
            ciscoDevice.currentPool = args[2];

            return true;
        }
        else if (args.Length == 4 && args[0].ToLower() == "route")
        {

            if (SubnetDictionary.IsValidIPAddress(args[1]) && SubnetDictionary.IsValidIPAddress(args[3]))
            {
                if (SubnetDictionary.getPrefix(args[2]) != "/?")
                {

                    StaticRoute sr = new StaticRoute();
                    sr.network = args[1] + SubnetDictionary.getPrefix(args[2]);
                    sr.route = args[3];
                    ciscoDevice.staticRoutes.Add(sr);
                    //TerminalConsoleBehavior.instance.currentObj.GetComponent<RouterBehavior>().routeAddresses.Add(args[1]);
                    //TerminalConsoleBehavior.instance.currentObj.GetComponent<RouterBehavior>().routeSubnets.Add(args[2]);
                    //TerminalConsoleBehavior.instance.currentObj.GetComponent<RouterBehavior>().routeDestinations.Add(args[3]);
                }
                else
                {
                    TerminalConsoleBehavior.printToTerminal("Invalid subnet");
                    return false;
                }
                return true;
            }
            else
            {
                TerminalConsoleBehavior.printToTerminal("Invalid IP Address");
                return false;
            }

        }
        else if (args.Length == 3 && args[0].ToLower() == "host")
        {
            if (SubnetDictionary.IsValidIPAddress(args[2]))
            {
                if (!ciscoDevice.DNSHosts.ContainsKey(args[1]))
                {
                    ciscoDevice.DNSHosts.Add(args[1], args[2]);
                    return true;
                }
                else
                {
                    ciscoDevice.DNSHosts[args[1]] = args[2];
                    return true;
                }
            }
            else
            {
                Debug.Log("DNS host invalid IP");
                return false;
            }
          
           
        }
        else if (args.Length == 2 && args[0].ToLower() == "name-server")
        {
            ciscoDevice.DNSName = args[1];
            return true;
        }
        else
        {
            return false;
        }
    }
}
