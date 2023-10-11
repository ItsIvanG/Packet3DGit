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
                    ghost.GetComponentInChildren<Renderer>().material.SetColor("_EmissionColor", new Color(0, 0.5f, 0.5f, 1));
                    Debug.Log("Ghost intantiated");

                }
                else if (ghost != null)
                {
                    ghost.transform.position = hit.point;
                    hitPosition = hit.point;

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
            if (ghost != null)
            {
                GameObject spawn = Instantiate(item.GameObject,transform);
                spawn.transform.position = hitPosition;
                spawn.transform.parent = null;
                spawn.name = item.Name;


                Destroy(ghost);
                Debug.Log(item+ " Item placed");
                item = null;

                
                orbit.desiredPosition = spawn.transform.position;
                orbit.desiredY = spawn.transform.position.y;
                
            }
        }

    }

    public void setPacketItem(PacketItem item)
    {
        this.item = item;
        Destroy(ghost);
        Debug.Log("setPacketItem success");
    }
}
