using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class cableRender : MonoBehaviour
{
    public LineRenderer lineRenderer;
    public Transform posA;
    public Transform posB;
    public GameObject cableLengthUI;
    public GameObject AStatus;
    public GameObject BStatus;
    Camera cam;
    public int measureUnit;
    TMP_Dropdown dropdown;
    private List<string> units= new List<string>{"","cm","m","\"","ft"};
    float convertedDistance;
    CableHops cableHops;
    CapsuleCollider CapsuleCol;
    public bool updateCollider=false;
    //0: disabled
    //1: cm
    //2: m
    //3: in
    //4: ft
    private void Awake()
    {
        cam = Camera.main;
        dropdown = GameObject.Find("MeasureDropdown").GetComponent<TMP_Dropdown>();
        cableHops = transform.parent.GetComponent<CableHops>();
        CapsuleCol = transform.parent.GetComponent<CapsuleCollider>();
    }
    void Update()
    {
        lineRenderer.SetPosition(0, posA.position);
        lineRenderer.SetPosition(1, posB.position);

        Vector3 midpoint = new Vector3(posA.position.x + (posB.position.x - posA.position.x) / 2, 
            posA.position.y + (posB.position.y - posA.position.y) / 2, 
            posA.position.z + (posB.position.z - posA.position.z) / 2);

        Vector3 AstatusPoint = new Vector3(posA.position.x + (posB.position.x - posA.position.x) * 0.2f,
           posA.position.y + (posB.position.y - posA.position.y) *0.2f,
           posA.position.z + (posB.position.z - posA.position.z) * 0.2f);

        Vector3 BstatusPoint = new Vector3(posA.position.x + (posB.position.x - posA.position.x) * 0.8f,
           posA.position.y + (posB.position.y - posA.position.y) * 0.8f,
           posA.position.z + (posB.position.z - posA.position.z) * 0.8f);

        cableLengthUI.transform.position = midpoint;
        cableLengthUI.transform.rotation = cam.transform.rotation;

        AStatus.transform.position = AstatusPoint;
        AStatus.transform.rotation = cam.transform.rotation;

        BStatus.transform.position = BstatusPoint;
        BStatus.transform.rotation = cam.transform.rotation;
        //cableLengthUI.transform.localScale = FindAnyObjectByType<orbitCam>().scrollVect*50;

        if (updateCollider)
        {
            CapsuleCol.transform.position = (posA.position + posB.position) * 0.5f;
            CapsuleCol.transform.LookAt(posB.position);
            CapsuleCol.height = Vector3.Distance(posA.position, posB.position);

            updateRender();
        }


        

    }
    public void Measure()
    {
        measureUnit = dropdown.value;
        if (measureUnit == 0)
        {
            cableLengthUI.SetActive(false);
        }
        else
        {
            float measuredDistanceInMeters = Vector3.Distance(posA.position, posB.position);
            switch (measureUnit)
            {
                case 1:
                    convertedDistance = measuredDistanceInMeters * 100;
                    break;
                case 2:
                    convertedDistance = measuredDistanceInMeters;
                    break;
                case 3:
                    convertedDistance = measuredDistanceInMeters * 39.3701f;
                    break;
                case 4:
                    convertedDistance = measuredDistanceInMeters * 3.28f;
                    break;
            }


            cableLengthUI.SetActive(true);
            cableLengthUI.GetComponentInChildren<TextMeshPro>().text = convertedDistance.ToString("0.00") + units[measureUnit];
        }
    }

    public void updateRender()
    {
        posA.transform.parent.transform.position = cableHops.portA.transform.position;
        posA.transform.parent.transform.rotation = cableHops.portA.transform.rotation;

        posB.transform.parent.transform.position = cableHops.portB.transform.position;
        posB.transform.parent.transform.rotation = cableHops.portB.transform.rotation;
    }
    
}
