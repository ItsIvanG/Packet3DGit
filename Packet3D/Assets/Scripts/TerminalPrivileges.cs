using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerminalPrivileges : MonoBehaviour
{
    public enum privileges { loggedOut,user,privileged,config,all};
    public static string[] priviledgePrefix = { ":",">","#","#" };
    public enum specificConfig { global,Interface,InterfaceRange,line };
    
    public static string[] configPrefix = { "config", "config-if","config-if-range","config-line" };
}
