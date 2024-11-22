using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class TutorialWait 
{
    //public bool waitForMe = false;
    //public string test;

    public GameObject arrowOn;
    public GameObject enableObjects;
    public enum waitFor { 
        None,
        Port_IPAdd,
        PC_State,
        Porthop_Obj,
        Cisco_Hostname,
        Cisco_Priviledge,
        PC_Ping,
        Cisco_Username,
        Cisco_Password,
        Cisco_EnablePassword,
        Cisco_SpecificConfig,
        Cisco_LoginLocal,
        Cisco_Banner,
        Cisco_InterfacePort,
        Cisco_NoShut,
        Cisco_WriteMem,
        Cisco_DNSHost,
        Cisco_DNSName,
        PC_Gateway,
        Port_PorthopSpecific,
        DHCP_Pool,
        DHCP_Net,
        DHCP_DefGate,
        DHCP_DNS,
        DHCP_Exc, //split 'begin' and 'end' by SPACE
        Port_DHCP,
        Cisco_StaticRoute, //two addresses <network> <route> (192.168.1.1/24 192.168.1.1)
        Cisco_RIPRoute,
        Cisco_FindPort,
        Cisco_EncapsulatedPort,
        Cisco_SubportIP,
        Cisco_RangeCheckVLAN,
        Cisco_CheckPortTrunk,
        Cisco_LoginLocalVTY,
        PC_SuccessTelnet

    };
    public waitFor waitType;
    public GameObject waitObject;
    public string fieldCheck;
    public bool boolCheck;
    public GameObject objectCheck;
    public PCBehavior.CurrentMenu PCStateCheck;
    public TerminalPrivileges.privileges CiscoPrivilegeCheck;
    public TerminalPrivileges.specificConfig CiscoSpecificConfigCheck;
    public TerminalPrivileges.lineConfig CiscoLineConfigCheck;
    public List<CiscoEthernetPort> RangePortCheck;

    public bool testWait()
    {
        //Debug.Log("Testing wait!");
        if (waitObject)
        {
            waitObject.TryGetComponent<CiscoDevice>(out CiscoDevice cd);
            waitObject.TryGetComponent<PCBehavior>(out PCBehavior pc);
            waitObject.TryGetComponent<PortProperties>(out PortProperties pp);
            waitObject.TryGetComponent<CiscoEthernetPort>(out CiscoEthernetPort cep);

            if (pc == null && cd == null && pp == null)
            {
                Debug.Log("Object check does not have components needed");
                return false;
            }


            switch (waitType)
            {
                case waitFor.DHCP_Pool:

                    foreach(var pool in cd.DHCPPools)
                    {
                        if (pool.DHCPName == fieldCheck) return true;
                    }
                     return false;

                case waitFor.DHCP_Net:

                    foreach (var pool in cd.DHCPPools)
                    {
                        if (pool.network == fieldCheck) return true;
                    }
                    return false;

                case waitFor.DHCP_DefGate:

                    foreach (var pool in cd.DHCPPools)
                    {
                        if (pool.defaultGateway == fieldCheck) return true;
                    }
                    return false;

                case waitFor.DHCP_DNS:

                    foreach (var pool in cd.DHCPPools)
                    {
                        if (pool.defaultDNS == fieldCheck) return true;
                    }
                    return false;

                case waitFor.DHCP_Exc:

                    foreach (var exc in cd.DHCPExcludes)
                    {
                        if (exc.excludeBegin == fieldCheck.Split(" ")[0] &&
                            exc.excludeEnd == fieldCheck.Split(" ")[1]) return true;
                    }
                    return false;

                case waitFor.Port_IPAdd:
                    if(waitObject.TryGetComponent(out PortProperties pcep)){
                        if (pcep.address == fieldCheck.Split('/')[0])
                        {
                            if (SubnetDictionary.getPrefix(pcep.subnet) == "/" + fieldCheck.Split('/')[1])
                            {
                                return true;
                            }
                        }
                    }


                    return false;

                case waitFor.PC_Ping:
                    if (pc.successPing == fieldCheck)
                    {
                        return true;
                    }
                    return false;

                case waitFor.PC_SuccessTelnet:
                    if (pc.successTelnet == fieldCheck)
                    {
                        return true;
                    }
                    return false;
                case waitFor.Cisco_Hostname:

                    if (cd.hostname == fieldCheck)
                        return true;
                    else return false;

                case waitFor.Cisco_Password:

                    if (cd.localPassword == fieldCheck)
                        return true;
                    else return false;

                case waitFor.Cisco_Username:

                    if (cd.localUsername == fieldCheck)
                        return true;
                    else return false;

                case waitFor.Cisco_EnablePassword:

                    if (cd.enablePassword == fieldCheck)
                        return true;
                    else return false;

                case waitFor.Porthop_Obj:

                    if (pp.portHopParent == objectCheck)
                        return true;
                    else return false;

                case waitFor.Port_PorthopSpecific:

                    if (pp.portHop == objectCheck.GetComponent<PortProperties>())
                        return true;
                    else return false;
                case waitFor.PC_State:

                    if (pc.currentMenu == PCStateCheck)
                        return true;
                    else return false;


                case waitFor.Cisco_Priviledge:

                    if (cd.currentPrivilege == CiscoPrivilegeCheck)
                        return true;
                    else return false;

                case waitFor.Cisco_SpecificConfig:

                    if (cd.currentConfigLevel == CiscoSpecificConfigCheck && cd.lineConfig == CiscoLineConfigCheck)
                        return true;
                    else return false;

                case waitFor.Cisco_LoginLocal:

                    if (cd.checkLoginLocal())
                        return true;
                    else return false;

                case waitFor.Cisco_LoginLocalVTY:

                    if (cd.checkLoginLocalVTY())
                        return true;
                    else return false;

                case waitFor.Cisco_Banner:

                    if (cd.MOTD == fieldCheck)
                        return true;
                    else return false;

                case waitFor.Cisco_InterfacePort:

                    if (cd.interfacePort == objectCheck.GetComponent<CiscoEthernetPort>())
                        return true;
                    else return false;

                case waitFor.Cisco_NoShut:

                    if (cep.noShut == boolCheck)
                        return true;
                    else return false;

                case waitFor.Cisco_WriteMem:

                    if (cd.writeMems.Count>0)
                        return true;
                    else return false;

                case waitFor.Cisco_DNSHost:

                    if (cd.DNSHosts.ContainsKey(fieldCheck.Split(" ")[0]) &&
                        cd.DNSHosts[fieldCheck.Split(" ")[0]] == fieldCheck.Split(" ")[1])
                        return true;
                    else return false;

                case waitFor.Cisco_DNSName:

                    if (cd.DNSName == fieldCheck)
                        return true;
                    else return false;

                case waitFor.PC_Gateway:

                    if (waitObject.TryGetComponent(out PCEthernetProperties pcepp))
                    {
                        if (pcepp.defaultgateway == fieldCheck)
                        {
                            return true;
                        }
                    }


                    return false;

                case waitFor.Port_DHCP:

                    if (waitObject.TryGetComponent(out PCEthernetProperties pceppp))
                    {
                        if (pceppp.isStaticIP == false)
                        {
                            return true;
                        }
                    }
                    return false;

                case waitFor.Cisco_StaticRoute:

                    var staticRoutes = cd.staticRoutes;

                    foreach(var route in staticRoutes)
                    {
                        if (route.network == fieldCheck.Split(" ")[0] && route.route == fieldCheck.Split(" ")[1]) return true;
                    }

                    return false;
                case waitFor.Cisco_RIPRoute:

                    var ripRoutes = cd.RIPNetworks;

                    foreach (var route in ripRoutes)
                    {
                        if (route == fieldCheck) return true;
                    }

                    return false;

                case waitFor.Cisco_FindPort:

                   var subports =  waitObject.GetComponentsInChildren<Subport>();
                    foreach(var subport in subports)
                    {
                        if (subport.gameObject.name == fieldCheck)
                        return true;
                    }

                    return false;

                case waitFor.Cisco_EncapsulatedPort:
                    var subportss = waitObject.GetComponentsInChildren<Subport>();
                    Subport sp = null; 
                    foreach (var subport in subportss)
                    {
                        if (subport.gameObject.name == fieldCheck)
                        {
                            sp = subport;
                            if (sp.isEnscapsulated && sp.encapsulationVlan == int.Parse(fieldCheck.Split(".")[1])) return true;

                        }


                    }

                    return false;

                case waitFor.Cisco_SubportIP:
                    var subportsss = waitObject.GetComponentsInChildren<Subport>();
                    Subport spp = null;
                    foreach (var subport in subportsss)
                    {
                        if (subport.gameObject.name == fieldCheck.Split(" ")[0])
                        {
                            spp = subport;
                            if (spp.address ==
                            fieldCheck.Split(" ")[1])
                                return true;

                        }


                    }

                    return false;

                case waitFor.Cisco_RangeCheckVLAN:

                    foreach(var port in RangePortCheck)
                    {
                        if (port.switchportAccessVlan != int.Parse(fieldCheck)) return false;
                    }

                    return true;

                case waitFor.Cisco_CheckPortTrunk:
                    if (cep.switchportMode == CiscoEthernetPort.switchportModes.Trunk) return true;

                    return false;

                default: return false;

            }
        }
        else
        {
            //Debug.Log("Tutorial line is a WAIT but no object assigned");
            return false;
        }

    }
}
