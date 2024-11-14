using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.UI;
using UnityEngine.XR.Management;
public class PropertiesTab : MonoBehaviour
{
    public static PropertiesTab instance;
    public Transform currentObj;
    public TextMeshProUGUI typeString;

    public Image thumbnailSprite;
    public TextMeshProUGUI nameString;
    public TextMeshProUGUI contextString;
    public PropertiesCanvas propertiesCanvas;
    public GameObject configButton;
    private XRUIInputModule InputModule => EventSystem.current.currentInputModule as XRUIInputModule;
    XRRayInteractor interactor;
    public void Start()
    {
        instance = this;
        instance.gameObject.SetActive(false);
    }


  

    // Update is called once per frame
    void Update()
    {
        
    }

    public  void updatePropertiesTab(Transform t)
    {
        instance.propertiesCanvas.openCanvas();
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
        else if(t != null)
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

    public void moveButton()
    {
        AddManager.instance.toMove = currentObj.gameObject;
        //AddManager.instance.toMove.gameObject.SetActive(false); 
        updatePropertiesTab(null);

        //AddManager.instance.setGrip(interactor.gameObject);
    }
    public void deleteButton()
    {
        Destroy(currentObj.gameObject);
        updatePropertiesTab(null);
    }
    public void closeButton()
    {
        updatePropertiesTab(null);
    }

    //public void OnPointerEnter(PointerEventData eventData)
    //{
    //    interactor = InputModule.GetInteractor(eventData.pointerId) as XRRayInteractor;

    //}
}
