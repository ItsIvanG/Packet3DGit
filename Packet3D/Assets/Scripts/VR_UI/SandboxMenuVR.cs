using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR;

public class SandboxMenuVR : MonoBehaviour
{
    public GameObject menu;
    public InputActionProperty showButton;
    public Transform leftControlTransform;
    Vector3 forwardLoc;
    public float menuDistance = 4f;
    public float lerpDamp = 2f;
    public float menuY = 1f;
    Transform cam;
    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main.transform;
    }

    // Update is called once per frame
    void Update()
    {
        if (showButton.action.WasPressedThisFrame())
        {
            Debug.Log("Menu press VR");
            menu.SetActive(!menu.activeSelf);
            forwardLoc = cam.forward.normalized * menuDistance;
            //menu.transform.position = cam.transform.position + new Vector3(cam.transform.forward.x,
            //    cam.transform.forward.y,
            //    cam.transform.forward.z).normalized * menuDistance;
            transform.position = leftControlTransform.position + (leftControlTransform.forward* menuDistance);
            transform.LookAt(new Vector3(cam.position.x, transform.position.y, cam.position.z));
           transform.Rotate(new Vector3(0, 10, 0));
            transform.forward *= -1;
        }

    
        //var lerpX = Mathf.Lerp(menu.transform.position.x, cam.position.x+ forwardLoc.x, Time.deltaTime*lerpDamp);
        //var lerpY = Mathf.Lerp(menu.transform.position.y, cam.position.y + forwardLoc.y+menuY, Time.deltaTime * lerpDamp);
        //var lerpZ = Mathf.Lerp(menu.transform.position.z, cam.position.z + forwardLoc.z, Time.deltaTime * lerpDamp);
        //menu.transform.position = new Vector3(lerpX, lerpY, lerpZ);
       


    }
}
