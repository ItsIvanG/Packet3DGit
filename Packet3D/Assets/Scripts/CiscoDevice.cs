using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VInspector;

public  class CiscoDevice : MonoBehaviour

{
    public  string hostname;
    public TerminalPrivileges.privileges currentPrivilege = TerminalPrivileges.privileges.loggedOut;
    public TerminalPrivileges.specificConfig currentConfigLevel = TerminalPrivileges.specificConfig.global;
    public TerminalPrivileges.lineConfig lineConfig = TerminalPrivileges.lineConfig.console;
    public string localUsername;
    public string localPassword;
    public string enablePassword;
    public string MOTD;
    public int enteringLocalWhat; //0: enter USERNAME, 1: enter PASSWORD, 2:no user and pass, just press RETURN
    public bool authenticatingEnable;
    //NOT ACCESSIBLE VIA TERMINALCONSOLEBEHAVIOR v v v v
    public CiscoEthernetPort interfacePort;
    public List<CiscoEthernetPort> interfaceRange;
    [Tooltip("Current vlan configuring (config-vlan)")]
    public Vlan configVlan;
    public int linConAvailable = 1;
    public List<LineConsole> lineConsoles;
    public List<LineConsole> lineVTYs;
    public int currentLineCon = -1;
    public List<float> writeMems;
    public List<StaticRoute> staticRoutes;
    public List<DHCPPool> DHCPPools;
    public List<DHCPExcludes> DHCPExcludes;
    public string currentPool;
    public SerializedDictionary<string, string> DNSHosts;
    public string DNSName;
    public List<string> RIPNetworks;


    [TextArea]
    public string terminalContent;

    public bool checkLoginLocal()
    {
        foreach(var lc in lineConsoles)
        {
            if (lc.usingLocal == true)
            {
                return true;
            }
        }
        return false;
    }

    public bool checkLoginLocalVTY()
    {
        foreach (var lc in lineVTYs)
        {
            if (lc.usingLocal == true)
            {
                return true;
            }
        }
        return false;
    }

}
