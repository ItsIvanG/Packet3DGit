using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CableHops : MonoBehaviour
{
    public PortTypes.Type portAType;
    public GameObject portA;
    public PortTypes.Type portBType;
    public GameObject portB;
    public bool bothHopValid;
    public Sprite upSprite; 
    public Sprite downSprite;
    public SpriteRenderer A_statusSprite; public SpriteRenderer B_statusSprite;
    public void UpdateHops(GameObject pA, GameObject pB)
    {
        portA = pA;
        portB = pB;

        PortProperties portAproperties = portA.GetComponent<PortProperties>();
        PacketItemPrefabDetails portAPrefabDetails = portA.GetComponentInParent<PacketItemPrefabDetails>();

        portAproperties.portHop = portB.GetComponent<PortProperties>();
        portAproperties.portHopParent = portB.transform.parent.gameObject;
        //portAPrefabDetails.UpdateContent();
        portAproperties.pluggedCable = gameObject;

        PortProperties portBproperties = portB.GetComponent<PortProperties>();
        PacketItemPrefabDetails portBPrefabDetails = portB.GetComponentInParent<PacketItemPrefabDetails>();

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
    private void Update()
    {
        if (A_statusSprite != null && B_statusSprite != null)
        {
            //TODO: HOP VALID SHOULD BE SEPARATE!
            if (bothHopValid)
            {
                A_statusSprite.sprite = upSprite;
                B_statusSprite.sprite = upSprite;
            }
            else
            {
                A_statusSprite.sprite = downSprite;
                B_statusSprite.sprite = downSprite;
            }
        }
           
    }

}
