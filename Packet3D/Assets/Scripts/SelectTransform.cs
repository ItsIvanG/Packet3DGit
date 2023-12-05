//MIT License
//Copyright (c) 2023 DA LAB (https://www.youtube.com/@DA-LAB)
//Permission is hereby granted, free of charge, to any person obtaining a copy
//of this software and associated documentation files (the "Software"), to deal
//in the Software without restriction, including without limitation the rights
//to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
//copies of the Software, and to permit persons to whom the Software is
//furnished to do so, subject to the following conditions:
//The above copyright notice and this permission notice shall be included in all
//copies or substantial portions of the Software.
//THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
//AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
//LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
//OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
//SOFTWARE.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using RuntimeHandle;
using UnityEngine.UI;
using TMPro;

public class SelectTransform : MonoBehaviour
{
    public Material highlightMaterial;
    public Material selectionMaterial;

    private Material originalMaterialHighlight;
    private Material originalMaterialSelection;
    public Transform highlight;
    public Transform selection;
    private RaycastHit raycastHit;
    private RaycastHit raycastHitHandle;
    public GameObject runtimeTransformGameObj;
    private RuntimeTransformHandle runtimeTransformHandle;
    private int runtimeTransformLayer = 6;
    private int runtimeTransformLayerMask;
    public GameObject MoveButton;
    public GameObject RotateButton;
    public GameObject DeleteButton;
    public GameObject WarnMessage;

    public Color highlightColor;
    public Color selectionColor;
    private bool deleteMode;
    public int timeSinceClick;
    private orbitCam orbit;


    private void Start()
    {
        runtimeTransformGameObj = new GameObject();
        runtimeTransformHandle = runtimeTransformGameObj.AddComponent<RuntimeTransformHandle>();
        runtimeTransformGameObj.layer = runtimeTransformLayer;
        runtimeTransformLayerMask = 1 << runtimeTransformLayer; //Layer number represented by a single bit in the 32-bit integer using bit shift
        runtimeTransformHandle.type = HandleType.POSITION;
        runtimeTransformHandle.autoScale = true;
        runtimeTransformHandle.autoScaleFactor = 1.0f;
        runtimeTransformGameObj.SetActive(false);
        MoveButton.GetComponent<Image>().color = UIColorManagerScript.ButtonActiveColor;
        orbit = FindAnyObjectByType<orbitCam>();
    }

    void Update()
    {
        // Highlight
        if (highlight != null)
        {
            highlight.GetComponentInChildren<Renderer>().material.DisableKeyword("_EMISSION");
            //highlight.GetComponentInChildren<MeshRenderer>().sharedMaterial = originalMaterialHighlight;
            highlight = null;
        }
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (!EventSystem.current.IsPointerOverGameObject() && Physics.Raycast(ray, out raycastHit)) //Make sure you have EventSystem in the hierarchy before using EventSystem
        {
            highlight = raycastHit.transform;
            if (highlight.CompareTag("Selectable") && highlight != selection)
            {
                //if (highlight.GetComponentInChildren<MeshRenderer>().material != highlightMaterial)
                //{
                //    originalMaterialHighlight = highlight.GetComponentInChildren<MeshRenderer>().material;
                //    highlight.GetComponentInChildren<MeshRenderer>().material = highlightMaterial;
                //}

                highlight.GetComponentInChildren<Renderer>().material.EnableKeyword("_EMISSION");
                highlight.GetComponentInChildren<Renderer>().material.SetColor("_EmissionColor", highlightColor);
            }
            else
            {
                highlight = null;
            }
        }

        // Selection
        if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject())
        {
            
            //DOUBLE CLICK TO FOCUS
            if (timeSinceClick == 0)
            {
                timeSinceClick = 1;
            }

           
            

            ApplyLayerToChildren(runtimeTransformGameObj);
            if (Physics.Raycast(ray, out raycastHit))
            {
                //PICK OBJ FOR SIMULATION

                if (SimulationBehavior.instance.isPicking)
                {
                    if (raycastHit.transform.GetComponent<PacketItemPrefabDetails>())
                    {
                        SimulationBehavior.instance.pickedObj(raycastHit.transform.gameObject);
                    }
                    else
                    {
                        SimulationBehavior.instance.CancelPick();
                    }
                }

                if (timeSinceClick > 5)
                {
                    //FOCUS ON OBJ SELECTION
                    orbit.desiredPosition = raycastHit.transform.position;
                    orbit.desiredY = raycastHit.transform.position.y;
                    Debug.Log("FOCUS ON " + raycastHit.transform.name);
                    timeSinceClick = 0;
                }

                if (Physics.Raycast(ray, out raycastHitHandle, Mathf.Infinity, runtimeTransformLayerMask)) //Raycast towards runtime transform handle only
                {
                }
                else if (highlight)
                {

                    
                    if (selection != null)
                    {
                        //selection.GetComponentInChildren<MeshRenderer>().material = originalMaterialSelection;
                        selection.GetComponentInChildren<Renderer>().material.DisableKeyword("_EMISSION");
                    }


                    selection = raycastHit.transform;
                    PropertiesTab.updatePropertiesTab(selection);

                    /*if (selection.GetComponentInChildren<MeshRenderer>().material != selectionMaterial)
                    {
                        originalMaterialSelection = originalMaterialHighlight;
                        selection.GetComponentInChildren<MeshRenderer>().material = selectionMaterial;
                        runtimeTransformHandle.target = selection;
                        runtimeTransformGameObj.SetActive(true);
                    }*/


                    selection.GetComponentInChildren<Renderer>().material.EnableKeyword("_EMISSION");
                    selection.GetComponentInChildren<Renderer>().material.SetColor("_EmissionColor", selectionColor);


                    //CHECK IF IT HAS 'NO TRANSFORM GIZMO' SCRIPT

                    if (!selection.GetComponent<NoTransformGizmo>())
                    {
                        runtimeTransformHandle.target = selection;
                        if (!deleteMode)
                        {
                            runtimeTransformGameObj.SetActive(true);
                        }
                        
                    }
                    if(deleteMode)
                    {
                        //DELETE MODE
                        WarnMessage.SetActive(true);
                        Debug.Log("ABOUT TO DELETE " + selection);
                        WarnMessage.transform.Find("warnMessageText").GetComponent<TextMeshProUGUI>().SetText("Are you sure you want to delete " + selection.name + "?");
                    }

                    highlight = null;
                }
                else
                {
                    if (selection) //CLICK ON OBJ BUT SHOULD BE NULL
                    {
                        //selection.GetComponentInChildren<MeshRenderer>().material = originalMaterialSelection;
                        selection.GetComponentInChildren<Renderer>().material.DisableKeyword("_EMISSION");
                        selection = null;
                        PropertiesTab.updatePropertiesTab(null);
                        runtimeTransformGameObj.SetActive(false);
                    }
                }
            }
            else
            {
                if (selection) //CLICK ON VOID
                {
                    //selection.GetComponentInChildren<MeshRenderer>().material = originalMaterialSelection;
                    selection.GetComponentInChildren<Renderer>().material.DisableKeyword("_EMISSION");
                    selection = null;
                    PropertiesTab.updatePropertiesTab(null);

                    runtimeTransformGameObj.SetActive(false);
                }
            }
        }

        if (timeSinceClick > 0 && timeSinceClick <80)
        {
            timeSinceClick++;
        }
        else
        {
            timeSinceClick = 0;
        }

        //Hot Keys for move, rotate, scale, local and Global/World transform
        /*if (runtimeTransformGameObj.activeSelf)
        {
            if (Input.GetKeyDown(KeyCode.W))
            {
                runtimeTransformHandle.type = HandleType.POSITION;
            }
            if (Input.GetKeyDown(KeyCode.E))
            {
                runtimeTransformHandle.type = HandleType.ROTATION;
            }
            if (Input.GetKeyDown(KeyCode.R))
            {
                runtimeTransformHandle.type = HandleType.SCALE;
            }
            if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
            {
                if (Input.GetKeyDown(KeyCode.G))
                {
                    runtimeTransformHandle.space = HandleSpace.WORLD;
                }
                if (Input.GetKeyDown(KeyCode.L))
                {
                    runtimeTransformHandle.space = HandleSpace.LOCAL;
                }
            }
        }*/

    }

    public void setTransformMode(int x)
    {
        switch (x)
        {
            case 0:
                if (selection != null)
                {
                    runtimeTransformGameObj.SetActive(true);
                }
                runtimeTransformHandle.type = HandleType.POSITION;
                MoveButton.GetComponent<Image>().color = UIColorManagerScript.ButtonActiveColor;
                RotateButton.GetComponent<Image>().color = UIColorManagerScript.ButtonIdleColor;
                DeleteButton.GetComponent<Image>().color = UIColorManagerScript.ButtonDeleteIdleColor;
                deleteMode = false;
                break;
            case 1:
                if (selection != null)
                {
                    runtimeTransformGameObj.SetActive(true);
                }
                runtimeTransformHandle.type = HandleType.ROTATION;
                MoveButton.GetComponent<Image>().color = UIColorManagerScript.ButtonIdleColor;
                RotateButton.GetComponent<Image>().color = UIColorManagerScript.ButtonActiveColor;
                DeleteButton.GetComponent<Image>().color = UIColorManagerScript.ButtonDeleteIdleColor;
                deleteMode = false;
                break;
            case 2:
                //DELETE MODE
                runtimeTransformGameObj.SetActive(false);
                MoveButton.GetComponent<Image>().color = UIColorManagerScript.ButtonIdleColor;
                RotateButton.GetComponent<Image>().color = UIColorManagerScript.ButtonIdleColor;
                DeleteButton.GetComponent<Image>().color = UIColorManagerScript.ButtonDeleteActiveColor;
                deleteMode = true;
                break;
        }
    }//

    public void cancelDelete()
    {
        selection.GetComponentInChildren<Renderer>().material.DisableKeyword("_EMISSION");
        selection = null;

    }

    public void continueDelete()
    {
        if (selection.GetComponent<CableHops>())
        {
            var ch = selection.GetComponent<CableHops>();
            ch.portA.GetComponent<PortProperties>().portHop = null;
            ch.portA.GetComponent<PortProperties>().portHopParent = null;
            ch.portB.GetComponent<PortProperties>().portHop = null;
            ch.portB.GetComponent<PortProperties>().portHopParent = null;
        }
        Destroy(selection.gameObject);
        SimulationBehavior.refreshDelay();

    }


    private void ApplyLayerToChildren(GameObject parentGameObj)
    {
        foreach (Transform transform1 in parentGameObj.transform)
        {
            int layer = parentGameObj.layer;
            transform1.gameObject.layer = layer;
            foreach (Transform transform2 in transform1)
            {
                transform2.gameObject.layer = layer;
                foreach (Transform transform3 in transform2)
                {
                    transform3.gameObject.layer = layer;
                    foreach (Transform transform4 in transform3)
                    {
                        transform4.gameObject.layer = layer;
                        foreach (Transform transform5 in transform4)
                        {
                            transform5.gameObject.layer = layer;
                        }
                    }
                }
            }
        }
    }

}