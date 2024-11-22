using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class CableRack : MonoBehaviour
{
    public bool spawnEnable = false,spawnAtStart = false;
    public GameObject cableToSpawn;
    [Header("Sockets")]
    public GameObject SocketFront_1;
    public GameObject SocketFront_2;
    public GameObject SocketBack_1;
    public GameObject SocketBack_2;
    private XRSocketInteractor sFront_1, sFront_2, sBack_1,sBack_2;
    //public getTexts;

    public bool addLenghts = true;
    private void Start()
    {
        sFront_1 = SocketFront_1.GetComponent<XRSocketInteractor>();
        sFront_2 = SocketFront_2.GetComponent<XRSocketInteractor>();

        sBack_1 = SocketBack_1.GetComponent<XRSocketInteractor>();
        sBack_2 = SocketBack_2.GetComponent<XRSocketInteractor>();

        if (spawnAtStart)
        {
            Invoke("spawnFront", 0.8f);
            Invoke("spawnBack", 1f);
        }
        if (addLenghts)
        {
            TextMeshPro[] getTexts = GetComponentsInChildren<TextMeshPro>();
            Debug.Log("Adding length to " + getTexts);
            string length="";
            if (cableToSpawn.name.Contains("MEDIUM"))
            {
                length = "2.5 m";
            } 
            else if (cableToSpawn.name.Contains("SHORT"))
            {
                length = "1.0 m";
            }

            foreach (var text in getTexts)
            {

                text.text+="\n"+length;
            }
        }
    }

    public void checkSockets()
    {
        Debug.Log("checking cable rack");
        Debug.Log("front 1: " + sFront_1.interactablesSelected.Count);
        Debug.Log("front 2: " + sFront_2.interactablesSelected.Count);
        if (spawnEnable)
        {
            if (sFront_1.interactablesSelected.Count <= 0 && sFront_2.interactablesSelected.Count <= 0)
            {
                Invoke("spawnFront", 1f);
            }
            else
            {
                CancelInvoke("spawnFront");
            }

            if (sBack_1.interactablesSelected.Count <= 0 && sBack_2.interactablesSelected.Count <= 0)
            {
                Invoke("spawnBack", 1f);
            }
            else
            {
                CancelInvoke("spawnBack");
            }
        }
       
    }

    void spawnFront()
    {
        if (sFront_1.interactablesSelected.Count <= 0 && sFront_2.interactablesSelected.Count <= 0)
        {
            GameObject cable = Instantiate(cableToSpawn);
            var wireEnds = cable.GetComponentsInChildren<WireEnd>();
            foreach (var we in wireEnds)
            {
                switch (we.whatEnd)
                {
                    case WireEnd.WireEndPort.A:
                        we.transform.position = SocketFront_1.transform.position;
                        break;
                    case WireEnd.WireEndPort.B:
                        we.transform.position = SocketFront_2.transform.position;
                        break;
                }
            }
        }
    }

    void spawnBack()
    {
        if (sBack_1.interactablesSelected.Count <= 0 && sBack_2.interactablesSelected.Count <= 0)
        {
            GameObject cable = Instantiate(cableToSpawn);
            var wireEnds = cable.GetComponentsInChildren<WireEnd>();
            foreach (var we in wireEnds)
            {
                switch (we.whatEnd)
                {
                    case WireEnd.WireEndPort.A:
                        we.transform.position = SocketBack_1.transform.position;
                        break;
                    case WireEnd.WireEndPort.B:
                        we.transform.position = SocketBack_2.transform.position;
                        break;
                }
            }
        }
    }
}
