using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public  class CiscoDevice : MonoBehaviour

{
    public  string hostname;
    public TerminalPrivileges.privileges currentPrivilege = TerminalPrivileges.privileges.loggedOut;
    public TerminalPrivileges.specificConfig currentConfigLevel = TerminalPrivileges.specificConfig.global;
    public string localUsername;
    public string localPassword;
    public string enablePassword;
    public string MOTD;
    public int enteringLocalWhat; //0: enter USERNAME, 1: enter PASSWORD, 2:no user and pass, just press RETURN
    public bool authenticatingEnable;
    //NOT ACCESSIBLE VIA TERMINALCONSOLEBEHAVIOR v v v v
    public CiscoEthernetPort interfacePort;
    public List<CiscoEthernetPort> interfaceRange;
    public Vlan configVlan;

    [TextArea]
    public string terminalContent;

}
