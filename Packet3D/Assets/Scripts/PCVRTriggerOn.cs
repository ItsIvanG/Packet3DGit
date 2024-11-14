using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PCVRTriggerOn : MonoBehaviour
{

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player") && transform.parent.gameObject!= DesktopCanvasScript.instance.currentPC)
        {
            //nilipat from ontrigger exit
            if (TerminalConsoleBehavior.instance.currentObj != null) TerminalConsoleBehavior.instance.saveVarsToCisco();
            TerminalConsoleBehavior.instance.currentObj = null;
            TerminalConsoleBehavior.instance.TerminalCanvas.SetActive(false);
            DesktopCanvasScript.instance.IPPanel.SetActive(false);
            DesktopCanvasScript.instance.gameObject.SetActive(false);
            TerminalConsoleBehavior.instance.inputField.text = "";
            //

            Debug.Log("PC enter");

         
            DesktopCanvasScript.showDesktopCanvas(transform.parent.gameObject);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
           
        }


    }
}
