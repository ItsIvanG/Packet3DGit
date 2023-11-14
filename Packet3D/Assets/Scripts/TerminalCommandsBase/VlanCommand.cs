using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "New VlanCommand", menuName = "Terminal/VlanCommand")]

public class VlanCommand : ConsoleCommand
{
    public GameObject vlanPrefab;
    public override bool Process(string[] args)
    {
        CiscoDevice ciscoDevice = TerminalConsoleBehavior.instance.currentObj.GetComponent<CiscoDevice>();
        if (args.Length == 1)
        {

            if(int.TryParse(args[0], out int vlanInt))
            {
                Vlan vlanComponent = null;
                Transform vlanTransform = TerminalConsoleBehavior.instance.currentObj.transform.Find("VLAN" + args[0]);
                if (vlanTransform == null)
                {
                    Debug.Log("Creating vlan " + args[0]);
                    GameObject vlan = Instantiate(vlanPrefab, TerminalConsoleBehavior.instance.currentObj.transform);
                    vlan.name = "VLAN" + args[0];
                    vlanComponent = vlan.GetComponent<Vlan>();
                    vlanComponent.PortName = "VLAN" + args[0];
                    vlanComponent.enabled = true;
                    vlanComponent.vlanNumber = int.Parse(args[0]);
                    PropertiesTab.updatePropertiesTab(TerminalConsoleBehavior.instance.currentObj.transform);
                }
                else
                {
                    Debug.Log("Vlan exists, accessing.");
                    vlanComponent = vlanTransform.GetComponent<Vlan>();
                }

                TerminalConsoleBehavior.instance.currentConfigLevel = TerminalPrivileges.specificConfig.vlan;
                ciscoDevice.configVlan = vlanComponent;
                return true;
            }
            else
            {
                return false;
            }
            
        }
        else
        {
            return false;
        }
    }
}