using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerminalPrivileges : MonoBehaviour
{
    public enum privileges { loggedOut,user,privileged,config,all,cmd};
    public static string[] priviledgePrefix = { ":",">","#","#" };
    public enum specificConfig { global,Interface,InterfaceRange,line,dhcppool,vlan,router,subInterface };
    public enum lineConfig { console,vty};
    
    public static string[] configPrefix = { "config", "config-if","config-if-range","config-line","dhcp-config","config-vlan","config-router","config-subif" };
}
