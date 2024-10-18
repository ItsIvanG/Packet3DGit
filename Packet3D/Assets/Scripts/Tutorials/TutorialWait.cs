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
        PC_IPAdd,
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
    };
    public waitFor waitType;
    public GameObject waitObject;
    public string fieldCheck;
    public GameObject objectCheck;
    public PCBehavior.CurrentMenu PCStateCheck;
    public TerminalPrivileges.privileges CiscoPrivilegeCheck;
    public TerminalPrivileges.specificConfig CiscoSpecificConfigCheck;
    public TerminalPrivileges.lineConfig CiscoLineConfigCheck;

    public bool testWait()
    {
        //Debug.Log("Testing wait!");
        if (waitObject)
        {
            waitObject.TryGetComponent<CiscoDevice>(out CiscoDevice cd);
            waitObject.TryGetComponent<PCBehavior>(out PCBehavior pc);
            waitObject.TryGetComponent<PortProperties>(out PortProperties pp);

            if (pc == null && cd == null && pp == null)
            {
                Debug.Log("Object check does not have components needed");
                return false;
            }


            switch (waitType)
            {
                case waitFor.PC_IPAdd:
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
