using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class orbitCam : MonoBehaviour
{

    Vector3 localRot = new Vector3(0, 20f, 0);
    public float movementSpeed = 0.7f;
    public float mouseSpeed = 3.0f;
    public float orbitDamping = 10f;
    public float scrollDamping =10f;
    public float positionDamping = 10f;
    public float mouseScrollSpeed = 2.0f;
    public GameObject cam;
    public Vector3 scrollVect = new Vector3(0.3f,0.3f,0.3f);
    public Vector3 desiredPosition = new Vector3(0, 0, 0);
    public float desiredY =0;

    
    void Start()
    {
    
    }

    void FixedUpdate()
    {
        
        //transform.position += transform.forward* Input.GetAxisRaw("Vertical") * sensitivity;
        //transform.position += transform.right * Input.GetAxisRaw("Horizontal") * sensitivity;
        // LOCK TO Y=0
        //transform.position = new Vector3(transform.position.x,0, transform.position.z);
        
        // WASD
        desiredPosition += transform.forward * Input.GetAxisRaw("Vertical") * movementSpeed * transform.localScale.x;
        desiredPosition += transform.right * Input.GetAxisRaw("Horizontal") * movementSpeed * transform.localScale.x;

        // Y
        if (Input.GetKey("q"))
        {
            //Debug.Log("DOWN");
            desiredY -= movementSpeed * 0.5f * transform.localScale.y ;
        } 
        else if (Input.GetKey("e"))
        {
            //Debug.Log("UP");
            desiredY += movementSpeed * 0.5f * transform.localScale.y;
        }
        desiredPosition = new Vector3(desiredPosition.x, desiredY, desiredPosition.z);

    }

    

    private void Update()
    {
        //SCROLL
        scrollVect *= -Input.GetAxisRaw("Mouse ScrollWheel") * mouseScrollSpeed+1;
        //OLD CODE
        //cam.transform.position += transform.forward * Input.GetAxisRaw("Mouse ScrollWheel") * mouseScrollSpeed;

        transform.localScale = Vector3.Lerp(transform.localScale, scrollVect, scrollDamping*Time.deltaTime);


        //ORBIT
        if (Input.GetMouseButton(1)&&!EventSystem.current.IsPointerOverGameObject())
        {
            localRot.x += Input.GetAxis("Mouse X") * mouseSpeed;
            localRot.y -= Input.GetAxis("Mouse Y") * mouseSpeed;

            localRot.y = Mathf.Clamp(localRot.y, -80f, 80f);

        }
        Quaternion QT = Quaternion.Euler(localRot.y, localRot.x, 0f);
        transform.rotation = Quaternion.Lerp(transform.rotation, QT, orbitDamping * Time.deltaTime);
        transform.position = Vector3.Lerp(transform.position, desiredPosition, positionDamping * Time.deltaTime);
    }
}
