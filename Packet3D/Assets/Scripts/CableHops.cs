using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class CableHops : MonoBehaviour
{
    public PortTypes.Type portAType;
    [Tooltip("Port GameObject with PortProperties THAT IT IS connected to.")]
    public GameObject portA;
    public PortTypes.Type portBType;
    [Tooltip("Port GameObject with PortProperties THAT IT IS connected to.")]
    public GameObject portB;
    [Tooltip("This wire's WireEndA")]
    public GameObject wireEndA;
    [Tooltip("This wire's WireEndB")]
    public GameObject wireEndB;
    public bool bothHopValid;
    public Sprite upSprite; 
    public Sprite downSprite;
    public SpriteRenderer A_statusSprite; public SpriteRenderer B_statusSprite;
    GameObject lastPortA, lastPortB;
    [Header("Start hops")]
    public XRSocketInteractor StartHopA;
      public XRSocketInteractor StartHopB;
    private void Awake()
    {
        if (StartHopA)
        {
            StartHopA.startingSelectedInteractable = wireEndA.GetComponent<XRGrabInteractable>();
            wireEndA.transform.position = StartHopA.transform.position;
            wireEndA.transform.rotation = StartHopA.transform.rotation;
        }
        if (StartHopB)
        {
            StartHopB.startingSelectedInteractable = wireEndB.GetComponent<XRGrabInteractable>();
            wireEndB.transform.position = StartHopB.transform.position;
            wireEndB.transform.rotation = StartHopB.transform.rotation;
        }
    }
    public void UpdateHops(GameObject pA, GameObject pB)
    {
        portA = pA;
        portB = pB;

        PortProperties portAproperties = portA.GetComponent<PortProperties>();
        //PacketItemPrefabDetails portAPrefabDetails = portA.GetComponentInParent<PacketItemPrefabDetails>();

        portAproperties.portHop = portB.GetComponent<PortProperties>();
        portAproperties.portHopParent = portB.transform.parent.gameObject;
        //portAPrefabDetails.UpdateContent();
        portAproperties.pluggedCable = gameObject;

        PortProperties portBproperties = portB.GetComponent<PortProperties>();
        //PacketItemPrefabDetails portBPrefabDetails = portB.GetComponentInParent<PacketItemPrefabDetails>();

        portBproperties.portHop = portA.GetComponent<PortProperties>();
        portBproperties.portHopParent = portA.transform.parent.gameObject;
        //portBPrefabDetails.UpdateContent();
        portBproperties.pluggedCable = gameObject;
        Debug.Log("Updated hops");

        if(A_statusSprite!=null && B_statusSprite != null)
        {
            A_statusSprite.enabled = true;
            B_statusSprite.enabled = true;
        }

    }

    public void UpdateHops() //PHYS WIRE
    {
        if(portA) lastPortA = portA;
        if (portB) lastPortB = portB;
        if (portA && portB)
        {
            
            lastPortB = portB;
            PortProperties portAproperties = portA.GetComponent<PortProperties>();
            //PacketItemPrefabDetails portAPrefabDetails = portA.GetComponentInParent<PacketItemPrefabDetails>();

            portAproperties.portHop = portB.GetComponent<PortProperties>();
            portAproperties.portHopParent = portB.transform.parent.gameObject;
            //portAPrefabDetails.UpdateContent();
            if ((portAproperties.portHopParent.GetComponent<PCBehavior>() ||
                portAproperties.portHopParent.GetComponent < RouterBehavior > () )&&
                 portAproperties.TryGetComponent<CiscoEthernetPort>(out CiscoEthernetPort cep))
            {
                cep.noShut = true;
            }
            portAproperties.pluggedCable = gameObject;

            PortProperties portBproperties = portB.GetComponent<PortProperties>();
            //PacketItemPrefabDetails portBPrefabDetails = portB.GetComponentInParent<PacketItemPrefabDetails>();

            portBproperties.portHop = portA.GetComponent<PortProperties>();
            portBproperties.portHopParent = portA.transform.parent.gameObject;
            //portBPrefabDetails.UpdateContent();
            if ((portBproperties.portHopParent.GetComponent<PCBehavior>() ||
                portBproperties.portHopParent.GetComponent<RouterBehavior>()) &&
              portBproperties.TryGetComponent<CiscoEthernetPort>(out CiscoEthernetPort cepb))
            {
                cepb.noShut = true;
            }
            portBproperties.pluggedCable = gameObject;
            Debug.Log("Updated hops");
        }
        else if (lastPortA && lastPortB)
        {
            PortProperties portAproperties = lastPortA.GetComponent<PortProperties>();

            portAproperties.portHop = null;
            portAproperties.portHopParent = null;
            portAproperties.pluggedCable = null;
            if (portAproperties.GetComponent<CiscoEthernetPort>() && !portAproperties.transform.parent.GetComponent<RouterBehavior>())
            {
                portAproperties.GetComponent<CiscoEthernetPort>().noShut = false;
            }

            PortProperties portBproperties = lastPortB.GetComponent<PortProperties>();

            portBproperties.portHop = null;
            portBproperties.portHopParent = null;
            portBproperties.pluggedCable = null;
            if (portBproperties.GetComponent<CiscoEthernetPort>() && !portBproperties.transform.parent.GetComponent<RouterBehavior>())
            {
                portBproperties.GetComponent<CiscoEthernetPort>().noShut = false;
            }
            lastPortA = null;
            lastPortB = null;
          
            Debug.Log("Updated hops");
        }
    }
    public void updateHopA(PortProperties pp)
    {
        if (pp)
            portA = pp.gameObject;
        else portA = null;
    }
    public void updateHopB(PortProperties pp)
    {
        if (pp)
            portB = pp.gameObject;
        else portB = null;
    }

    //private void Update()
    //{
    //    if (A_statusSprite != null && B_statusSprite != null)
    //    {
    //        //TODO: HOP VALID SHOULD BE SEPARATE!
    //        if (bothHopValid)
    //        {
    //            A_statusSprite.sprite = upSprite;
    //            B_statusSprite.sprite = upSprite;
    //        }
    //        else
    //        {
    //            A_statusSprite.sprite = downSprite;
    //            B_statusSprite.sprite = downSprite;
    //        }
    //    }

    //}

}
