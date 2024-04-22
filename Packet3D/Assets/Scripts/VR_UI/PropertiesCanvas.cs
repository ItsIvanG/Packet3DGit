using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PropertiesCanvas : MonoBehaviour
{
    public Canvas canvas;
    public GameObject rightControl;
    Transform cam;
    // Start is called before the first frame update
    private void Start()
    {
        cam = Camera.main.transform;
    }
    public void openCanvas()
    {
        RaycastHit hit;
        bool RightRay = Physics.Raycast(rightControl.transform.position, rightControl.transform.forward, out hit, 100);

        if (RightRay)
        {
            transform.position = hit.point;


            transform.LookAt(new Vector3(cam.position.x, transform.position.y, cam.position.z));
            transform.forward *= -1;
        }

    }
}
