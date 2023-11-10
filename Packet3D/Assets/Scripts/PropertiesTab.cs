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
        instance.currentObj = t;

        if (t == null || !t.GetComponent<PacketItemPrefabDetails>() )
        {
            instance.gameObject.SetActive(false);
        }
        else if (t.GetComponent<PacketItemPrefabDetails>().type == PacketItem.Type.Router || t.GetComponent<PacketItemPrefabDetails>().type == PacketItem.Type.EndDevice || t.GetComponent<PacketItemPrefabDetails>().type == PacketItem.Type.Switch)
        {
            instance.gameObject.SetActive(true);

            instance.thumbnailSprite.sprite = instance.currentObj.GetComponent<PacketItemPrefabDetails>().icon;
            instance.typeString.text = instance.currentObj.GetComponent<PacketItemPrefabDetails>().type.ToString();
            instance.nameString.text = instance.currentObj.GetComponent<PacketItemPrefabDetails>().name;


            instance.contextString.text = "";

            ///DISPLAY HOSTNAME
            if (instance.currentObj.GetComponent<RouterProperties>())
            {
                instance.contextString.text += "<style=\"h3\">Hostname:</style>\n" + instance.currentObj.GetComponent<RouterProperties>().hostname + "\n\n";
            }
            else if (instance.currentObj.GetComponent<SwitchProperties>())
            {
                instance.contextString.text += "<style=\"h3\">Hostname:</style>\n" + instance.currentObj.GetComponent<SwitchProperties>().hostname + "\n\n";
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
                instance.contextString.text += p.address + SubnetDictionary.getPrefix(p.subnet)+ "\n\n";
            }

        }
    }

    public void configureButton()
    {
        if(instance.currentObj.GetComponent<PacketItemPrefabDetails>().type== PacketItem.Type.EndDevice)
        {
            

            Debug.Log("opening pc desktop.."+instance.currentObj.gameObject);
            DesktopCanvasScript.showDesktopCanvas(instance.currentObj.gameObject);
            Debug.Log(" pc desktop open success");
        } else if (instance.currentObj.GetComponent<PacketItemPrefabDetails>().type == PacketItem.Type.Router || instance.currentObj.GetComponent<PacketItemPrefabDetails>().type == PacketItem.Type.Switch)
        {
            TerminalCanvasScript.ShowTerminal(instance.currentObj.gameObject);
        }
    }

    public void reUpdate()
    {
        updatePropertiesTab(instance.currentObj);
    }
}
