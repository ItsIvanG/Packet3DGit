using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

[CreateAssetMenu(fileName = "New InterfaceCommand Command", menuName = "Terminal/InterfaceCommand Command")]
public class InterfaceCommand : ConsoleCommand
{
    public GameObject vlanPrefab;
    public bool range;
    public int startRange;
    public int endRange;
    public override bool Process(string[] args)
    {
        CiscoDevice ciscoDevice = TerminalConsoleBehavior.instance.currentObj.GetComponent<CiscoDevice>();
        if (args.Length <2)
        {
            Debug.Log("accessing interface non-range");
            range = false;


           
            CiscoEthernetPort interfacePort=null;
            Debug.Log("Finding port: "+args[0] + " from " + TerminalConsoleBehavior.instance.currentObj);
            
            var getAllCiscoPorts  = TerminalConsoleBehavior.instance.currentObj.GetComponentsInChildren<CiscoEthernetPort>();

            
            foreach (CiscoEthernetPort port in getAllCiscoPorts)
            {

                //interface f0/1
                if (port.name == args[0].ToUpper())
                {
                    interfacePort = port;
                } 
                
            }

            if (interfacePort != null)
            {
                Debug.Log("Found port " + args[0]);
                ciscoDevice.interfacePort = interfacePort;
                TerminalConsoleBehavior.instance.currentConfigLevel = TerminalPrivileges.specificConfig.Interface;

                return true;
            }
            else
            {
                Debug.Log("Did not find port " + args[0]);
                TerminalConsoleBehavior.printToTerminal("Could not find port:" + args[0] + ".");
                return false;
            }
           
        }
        else if (args[0]=="range" && args.Length< 3)
        {
            string[] rangeSplit = args[1].Split("-");
            if (rangeSplit.Length == 1)
            {
                return false;
            }
            else if (rangeSplit.Length == 2)
            {
                //TODO: SUPPORT 2 DIGIT START RANGES!!
                Debug.Log("accessing interface range " + args[1][3] + "-" + rangeSplit[1]);
                startRange = (int)char.GetNumericValue(args[1][3]);
                endRange = int.Parse(rangeSplit[1]);
                range = true;



                var getAllCiscoPorts = TerminalConsoleBehavior.instance.currentObj.GetComponentsInChildren<CiscoEthernetPort>();
                if (range)
                {
                    foreach (CiscoEthernetPort port in getAllCiscoPorts)
                    {

                        for (int i = startRange; i <= endRange; i++)
                        {
                            //TODO: SUPPORT 2 DIGIT START RANGES!!
                            Debug.Log("(RANGE) attempting to find " + args[1].ToUpper().Substring(0, 3) + i);
                            if (port.name == args[1].ToUpper().Substring(0, 3) + i)
                            {
                                ciscoDevice.interfaceRange.Add(port);
                                TerminalConsoleBehavior.instance.currentConfigLevel = TerminalPrivileges.specificConfig.InterfaceRange;
                            }
                        }
                    }

                   
                }
                return true;
            }
            else
            {
                return false;
            }

        }
        else if (args[0]=="vlan" && args.Length < 3 && TerminalConsoleBehavior.instance.currentObj.GetComponent<SwitchBehavior>())
        {
            Vlan vlanComponent = null;
            Transform vlanTransform = TerminalConsoleBehavior.instance.currentObj.transform.Find("VLAN" + args[1]);
            if (vlanTransform == null){
                Debug.Log("Creating vlan " + args[1]);
                GameObject vlan = Instantiate(vlanPrefab, TerminalConsoleBehavior.instance.currentObj.transform);
                vlan.name = "VLAN"+args[1];
                vlanComponent = vlan.GetComponent<Vlan>();
                vlanComponent.PortName = "VLAN" + args[1];
                vlanComponent.enabled = true;
                vlanComponent.vlanNumber = int.Parse(args[1]);
                PropertiesTab.updatePropertiesTab(TerminalConsoleBehavior.instance.currentObj.transform);
            }
            else
            {
                Debug.Log("Vlan exists, accessing.");
                vlanComponent = vlanTransform.GetComponent<Vlan>();
            }
            TerminalConsoleBehavior.instance.currentConfigLevel = TerminalPrivileges.specificConfig.Interface;
            ciscoDevice.interfacePort = vlanComponent;
            return true;
        }
        else
        {
            return false;
        }
    }
}
