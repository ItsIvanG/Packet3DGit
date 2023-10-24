using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CableHops : MonoBehaviour
{
    public GameObject portA;
    public GameObject portB;

    public void UpdateHops(GameObject pA, GameObject pB)
    {
        portA = pA;
        portB = pB;

        portA.GetComponent<PortProperties>().portHop = portB.GetComponent<PortProperties>();
        portA.GetComponent<PortProperties>().portHopParent = portB.transform.parent.gameObject;
        portA.GetComponentInParent<PacketItemPrefabDetails>().UpdateContent();

        portB.GetComponent<PortProperties>().portHop = portA.GetComponent<PortProperties>();
        portB.GetComponent<PortProperties>().portHopParent = portA.transform.parent.gameObject;
        portB.GetComponentInParent<PacketItemPrefabDetails>().UpdateContent();

        Debug.Log("Updated hops");
    }

}
