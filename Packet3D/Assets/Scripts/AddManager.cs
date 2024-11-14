using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;

public class AddManager : MonoBehaviour
{
    Camera cam;
    public PacketItem item;
    public GameObject ghost;
    Vector3 hitPosition;
    orbitCam orbit;
    int addCableState=0;
    private GameObject spawn;
    Transform cablePosA;
    Transform cablePosB;
    GameObject cablePortGameObjectA;
    GameObject cablePortGameObjectB;
    public GameObject ControllerGameObj, LeftControlRef,RightControlRef;
    public InputActionProperty addGrip,rotateButton;
    public TextMeshProUGUI DebugText;
    public static AddManager instance;
    private bool raycastControl;
    public GameObject toMove;
    public GameObject LControlStable, RControlStable;
    // Start is called before the first frame update
    void Start()
    {
       cam = Camera.main;
       orbit = FindAnyObjectByType<orbitCam>();
        instance = this;

    }

    // Update is called once per frame
    void Update()
    {
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = 10f;
        mousePos = cam.ScreenToWorldPoint(mousePos);
        Debug.DrawRay(cam.transform.position,mousePos-cam.transform.position, Color.yellow);
        //Debug.Log(mousePos);

        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;


            raycastControl = Physics.Raycast(ControllerGameObj.transform.position, ControllerGameObj.transform.forward, out hit, 100);


        //DebugText.text = hit.collider.name;
        //check if item to add is not null
        if (item != null || toMove!=null)
        {
            
            //if (Physics.Raycast(ray, out hit, 100)) PC RAYCAST
            if (Physics.Raycast(ControllerGameObj.transform.position, ControllerGameObj.transform.forward, out hit, 100))
            {

                if (ghost == null)
                {
                    if (toMove == null){
                        ghost = Instantiate(item.GameObject, transform);
                    }
                    else
                    {
                        ghost = Instantiate(toMove,transform);
                        toMove.transform.position += toMove.transform.up * -100;
                        setGrip(1);
                    }

                    
                    var g = ghost.GetComponentsInChildren<Collider>();
                    foreach(var x in g)
                    {
                        x.enabled = false;
                    }
                    ghost.GetComponentInChildren<Renderer>().material.EnableKeyword("_EMISSION");
                    ghost.GetComponentInChildren<Renderer>().material.SetColor("_EmissionColor", new Color(0, 0.18f, 0.3f, 1));
                    Debug.Log("Ghost intantiated");

                }
                else if (ghost != null)
                {
                    ghost.transform.position = hit.point;
                    hitPosition = hit.point;
                    //if (item.type == PacketItem.Type.Cable)
                    //{
                    //    ghost.transform.rotation = Quaternion.FromToRotation(Vector3.forward, hit.normal);
                        
                    //}

                }
            }
            else//destroy ghost since raycast aint hitting anything
            {
                if (ghost != null)
                {
                    Destroy(ghost);
                    Debug.Log("Ghost destroyed");
                    
                }
            }
        }

        if (rotateButton.action.WasPressedThisFrame())
        {
            ghost.transform.Rotate(0, 45, 0);
        }
            //LEFT CLICK SPAWN
            //if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject()) ---- PC
            if (addGrip.action.WasPressedThisFrame())
        {
            Debug.Log("LMB PRESSED");
            //if (ghost != null && addCableState == 0)
            if (ghost != null)
            {
                if (toMove == null)
                {
                    spawn = Instantiate(item.GameObject);
                    spawn.transform.position = hitPosition;
                    spawn.transform.rotation = ghost.transform.rotation;
                    //spawn.transform.parent = null;
                    spawn.name = item.Name;
                }
                else
                {
                    toMove.transform.position = hitPosition;
                    toMove.transform.rotation = ghost.transform.rotation;
                    toMove.SetActive(true);
                    toMove = null;
                }
                
                SimulationBehavior.refreshDelay();
                enableAllCableCollisions();

                Destroy(ghost);
                Debug.Log(item + " Item placed");
                

                //CHECK IF CABLE
                //if (item.type == PacketItem.Type.Cable && addCableState == 0)
                //{
                //    cablePosA = spawn.transform.Find("portA");
                //    disableAllCableCollisions();
                    

                //   // if (Physics.Raycast(ray, out hit, 100) ---------- PC
                //   if(raycastControl
                //        && hit.collider.GetComponent<PortProperties>())
                //    {
                //        if (hit.collider.GetComponent<PortProperties>().PortType == spawn.GetComponent<CableHops>().portAType)
                //        {
                //            if (cablePosA != null)
                //            {


                //                spawn.transform.position = hit.collider.transform.position;
                //                cablePosA.transform.rotation = hit.collider.transform.rotation;
                //                cablePortGameObjectA = hit.collider.transform.gameObject;

                //                Debug.Log("cablePosA set");
                //                addCableState = 1;
                //                cablePosB = spawn.transform.Find("portB");
                //            }
                //        }
                //        else
                //        {
                //            Destroy(spawn);
                //            return;
                //        }  
                //    }
                //    else
                //    {
                //        Destroy(spawn);
                //        return;
                //    }
                //}


                item = null;
                //orbit.desiredPosition = spawn.transform.position;
                //orbit.desiredY = spawn.transform.position.y;
            }
            //else if (addCableState == 1)
            //{
                
            //    //if (Physics.Raycast(ray, out hit, 100) ----------- pc
            //      if (raycastControl

            //        && hit.collider.GetComponent<PortProperties>())
            //    {
            //        if (hit.collider.GetComponent<PortProperties>().PortType == spawn.GetComponent<CableHops>().portBType)
            //        {
            //            if (cablePosB != null)
            //            {
            //                cablePosB.transform.position = hit.collider.transform.position;
            //                cablePosB.transform.rotation = hit.collider.transform.rotation;
            //                cablePortGameObjectB = hit.collider.transform.gameObject;
            //                Debug.Log("cablePosB set");
            //                addCableState = 0;
            //                spawn.GetComponent<CableHops>().UpdateHops(cablePortGameObjectA, cablePortGameObjectB);
            //                spawn.GetComponentInChildren<cableRender>().updateCollider = true;
            //                enableAllCableCollisions();
            //            }
            //        }
            //        else
            //        {
            //            return;
            //        }
            //    }
            //    else
            //    {
            //        return;
            //    }
            //}
            
        }
        //Cable pos B ghost
        //if (addCableState == 1)
        //{ 
        //    if (raycastControl)
        //    {
        //        cablePosB.transform.position = hit.point;
        //        cablePosB.rotation = Quaternion.FromToRotation(Vector3.forward, hit.normal);
        //        spawn.GetComponentInChildren<cableRender>().Measure();
        //    }
        //}
    }

    public void setPacketItem(PacketItem item)
    {
        addCableState = 0;
        this.item = item;
        Destroy(ghost);
        Debug.Log("setPacketItem success");
    }
    public void setPacketItem(PacketItem item,int control)
    {
        addCableState = 0;
        this.item = item;
        Destroy(ghost);
        Debug.Log("setPacketItem success");
        setGrip(control);

    }

    public void disableAllCableCollisions()
    {
        Debug.Log("disable all cable collisions");
        var allCables = FindObjectsByType<CableHops>(0);
        foreach(var cable in allCables)
        {
            if (cable.GetComponent<CapsuleCollider>())
            {
                cable.GetComponent<CapsuleCollider>().enabled = false;
            }
        }
    }

    public void enableAllCableCollisions()
    {
        Debug.Log("enable all cable collisions");
        var allCables = FindObjectsByType<CableHops>(0);
        foreach (var cable in allCables)
        {
            if (cable.GetComponent<CapsuleCollider>())
            {
                cable.GetComponent<CapsuleCollider>().enabled = true;
            }
        }
    }
    public void setGrip(GameObject controller)
    {
        ControllerGameObj = controller;
     
    }
    public void setGrip(int control)
    {
        Debug.Log("Setting activate controller to " + control);
        if (control == 1)
        {
            ControllerGameObj = RControlStable;
            addGrip = RightControlRef.GetComponent<ActionBasedController>().activateAction;
        }
        else if (control == 2)
        {
            ControllerGameObj = LControlStable;
            addGrip = LeftControlRef.GetComponent<ActionBasedController>().activateAction;
        }
     
    }

}
