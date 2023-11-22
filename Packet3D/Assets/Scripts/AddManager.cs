using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class AddManager : MonoBehaviour
{
    Camera cam;
    public PacketItem item;
    GameObject ghost;
    Vector3 hitPosition;
    orbitCam orbit;
    int addCableState=0;
    private GameObject spawn;
    Transform cablePosA;
    Transform cablePosB;
    GameObject cablePortGameObjectA;
    GameObject cablePortGameObjectB;
    // Start is called before the first frame update
    void Start()
    {
       cam = Camera.main;
       orbit = FindAnyObjectByType<orbitCam>();

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


        //check if item to add is not null
        if (item != null)
        {
            if (Physics.Raycast(ray, out hit, 100))
            {

                if (ghost == null)
                {
                    ghost = Instantiate(item.GameObject, transform);
                    
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
                    if (item.type == PacketItem.Type.Cable)
                    {
                        ghost.transform.rotation = Quaternion.FromToRotation(Vector3.forward, hit.normal);
                        
                    }

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

        //LEFT CLICK SPAWN
        if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject())
        {
            //Debug.Log("LMB PRESSED");
            if (ghost != null && addCableState==0)
            {
                spawn = Instantiate(item.GameObject);
                spawn.transform.position = hitPosition;
                //spawn.transform.parent = null;
                spawn.name = item.Name;
                
                SimulationBehavior.refreshDelay();


                Destroy(ghost);
                Debug.Log(item + " Item placed");
                

                //CHECK IF CABLE
                if (item.type == PacketItem.Type.Cable && addCableState == 0)
                {
                    cablePosA = spawn.transform.Find("portA");
                    

                    if (Physics.Raycast(ray, out hit, 100) && hit.transform.GetComponent<PortProperties>())
                    {
                        if (cablePosA != null)
                        {
                            spawn.transform.position = hit.transform.position;
                            cablePosA.transform.rotation = hit.transform.rotation;
                            cablePortGameObjectA = hit.transform.gameObject;
                            
                            Debug.Log("cablePosA set");
                            addCableState = 1;
                            cablePosB = spawn.transform.Find("portB");
                        }
                    }
                }


                item = null;
                orbit.desiredPosition = spawn.transform.position;
                orbit.desiredY = spawn.transform.position.y;
            }
            else if (addCableState == 1)
            {
                
                if (Physics.Raycast(ray, out hit, 100))
                {
                    if (cablePosB != null)
                    {
                        cablePosB.transform.position = hit.transform.position;
                        cablePosB.transform.rotation = hit.transform.rotation;
                        cablePortGameObjectB = hit.transform.gameObject;
                        Debug.Log("cablePosB set");
                        addCableState = 0;
                        spawn.GetComponent<CableHops>().UpdateHops(cablePortGameObjectA, cablePortGameObjectB);
                        spawn.GetComponentInChildren<cableRender>().updateCollider = true;
                    }
                }
            }
            
        }
        //Cable pos B ghost
        if (addCableState == 1)
        { 
            if (Physics.Raycast(ray, out hit, 100))
            {
                cablePosB.transform.position = hit.point;
                cablePosB.rotation = Quaternion.FromToRotation(Vector3.forward, hit.normal);
                spawn.GetComponentInChildren<cableRender>().Measure();
            }
        }
    }

    public void setPacketItem(PacketItem item)
    {
        addCableState = 0;
        this.item = item;
        Destroy(ghost);
        Debug.Log("setPacketItem success");
    }
}
