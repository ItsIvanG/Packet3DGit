using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerminalPrivileges : MonoBehaviour
{
    public enum privileges { loggedOut,user,privileged,config,all};
    public static string[] priviledgePrefix = { ":",">","#","#" };
    public enum specificConfig { global,Interface,InterfaceRange,line,dhcppool,vlan };
    
    public static string[] configPrefix = { "config", "config-if","config-if-range","config-line","dhcp-config","config-vlan" };
}
