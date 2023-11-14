using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PropertiesTab : MonoBehaviour
{
    public static PropertiesTab instance;
    public Transform currentObj;
    public TextMeshProUGUI typeString;

    public Image thumbnailSprite;
    public TextMeshProUGUI nameString;
    public TextMeshProUGUI contextString;
    public void Awake()
    {
        instance = this;
        instance.gameObject.SetActive(false);
    }


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public static void updatePropertiesTab(Transform t)
    {
        PacketItemPrefabDetails currentObjPacketDetails = null; 
        instance.currentObj = t;
        if (t != null)
        {
            currentObjPacketDetails = instance.currentObj.GetComponent<PacketItemPrefabDetails>();
        }


        if (t == null || !currentObjPacketDetails)
        {
            instance.gameObject.SetActive(false);
        }
        else if (currentObjPacketDetails.type == PacketItem.Type.Router || 
            currentObjPacketDetails.type == PacketItem.Type.EndDevice || 
            currentObjPacketDetails.type == PacketItem.Type.Switch)
        {
            instance.gameObject.SetActive(true);

            instance.thumbnailSprite.sprite = currentObjPacketDetails.icon;
            instance.typeString.text = currentObjPacketDetails.type.ToString();
            instance.nameString.text = currentObjPacketDetails.name;


            instance.contextString.text = "";

            ///DISPLAY HOSTNAME
            if (instance.currentObj.GetComponent<CiscoDevice>())
            {
                instance.contextString.text += "<style=\"h3\">Hostname:</style>\n" + instance.currentObj.GetComponent<CiscoDevice>().hostname + "\n\n";
            }
           

            //DISPLAY PORTS
            var ports = instance.currentObj.GetComponentsInChildren<PortProperties>();

            foreach(var p in ports)
            {
                //"<style=\"subtle\">"
                

                instance.contextString.text += "<style=\"h3\">"+p.PortName+"</style>\n";
                if(p.portHop != null)
                {
                    instance.contextString.text += p.portHop.PortName + "(" + p.portHopParent.name + ")\n";
                }

                if (p is Vlan)
                {
                    Vlan vlan = (Vlan)p;
                    var vlanPorts = vlan.vlanPorts;
                    instance.contextString.text += "Name: " + vlan.vlanName+"\n";
                    foreach (var port in vlanPorts)
                    {
                        instance.contextString.text += port.PortName+"\n";
                    }
                    
                }

                if (p.address != "")
                {
                    instance.contextString.text += p.address + SubnetDictionary.getPrefix(p.subnet) + "\n";
                }
                instance.contextString.text += "\n";

            }

        }
    }

    public void configureButton()
    {
        PacketItemPrefabDetails currentObjPacketDetails = currentObj.GetComponent<PacketItemPrefabDetails>();
        if(currentObjPacketDetails.type== PacketItem.Type.EndDevice)
        {
            

            Debug.Log("opening pc desktop.."+instance.currentObj.gameObject);
            DesktopCanvasScript.showDesktopCanvas(instance.currentObj.gameObject);
            Debug.Log(" pc desktop open success");
        } else if (currentObjPacketDetails.type == PacketItem.Type.Router || currentObjPacketDetails.type == PacketItem.Type.Switch)
        {
            TerminalCanvasScript.ShowTerminal(instance.currentObj.gameObject);
        }
    }

    public void reUpdate()
    {
        updatePropertiesTab(instance.currentObj);
    }
}
