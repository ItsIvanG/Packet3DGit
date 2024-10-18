using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class SocketBehavior : MonoBehaviour
{
    WireEnd we;


    public void testPortSocket(GameObject me)
    {
         we = me.GetComponentInChildren<XRSocketInteractor>().firstInteractableSelected.
            transform.GetComponent<WireEnd>();
        PortProperties pp = me.GetComponent<PortProperties>();
        Debug.Log("VR Port attached: " + me.name);
        Debug.Log("Attached WireEnd: " + me.GetComponentInChildren<XRSocketInteractor>().interactablesSelected[0]);
        we.updateHop(pp);
        myDebugScripts.instance.UpdateAllHops();

    }
    public void removePortSocket()
    {
        we.updateHop(null);
        myDebugScripts.instance.UpdateAllHops();
    }
}
