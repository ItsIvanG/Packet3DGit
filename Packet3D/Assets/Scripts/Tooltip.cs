using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

//[ExecuteInEditMode()]
public class Tooltip : MonoBehaviour
{
    public TextMeshProUGUI headerField;
    public TextMeshProUGUI contentField;
    public TextMeshProUGUI subtitleField;
    public LayoutElement layoutElement;
    public int characterWraplimit;

    public RectTransform rectTransform;
    public GameObject rightControl;
    TooltipTrigger lastTooltip = null;
    Transform cam;
    LayerMask rayMask;
    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main.transform;
    }
    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        // Define the layer indices you want to exclude
        int[] exclusionLayerIndices = { 2, 7 }; // ALSO CHANGE IN TOOLTIPSYSTEM IF CHANGING

        // Initialize LayerMask to include all layers
        rayMask = ~0; // This means all layers

        foreach (int layerIndex in exclusionLayerIndices)
        {
            // Exclude the layer by using bitwise AND with the inverted layer
            rayMask &= ~(1 << layerIndex);
        }
    }
    public void SetText( string header, string content, string sub)
    {
        contentField.text = content;
        headerField.text = header;
        subtitleField.text = sub;

        int headerLength = headerField.text.Length;
        int contentLength = contentField.text.Length;

        //width limit
        layoutElement.enabled = (headerLength > characterWraplimit || contentLength > characterWraplimit) ? true : false;
    }

    //private void FixedUpdate()
    //{

    //    RaycastHit hit;
    //    bool RightRay = Physics.Raycast(rightControl.transform.position, rightControl.transform.forward, out hit, 100, rayMask);



    //    //if (RightRay && hasTooltip)
    //    //{
    //    //    transform.position = hit.collider.transform.position;
    //    //   

    //    //    transform.LookAt(new Vector3(cam.position.x, transform.position.y, cam.position.z));
    //    //    transform.forward *= -1;
    //    //    lastTooltip = hasTooltip;
    //    //}

    //    if (RightRay)
    //    {
    //        transform.position = hit.collider.transform.position;

    //        transform.LookAt(new Vector3(cam.position.x, transform.position.y, cam.position.z));
    //        transform.forward *= -1;
           

    //    }
    //    //else if ((!RightRay || !hasTooltip) && lastTooltip)
    //    //{
    //    //    lastTooltip.onRayLeave();
    //    //    lastTooltip = null;
    //    //}

    //    //Vector2 position;
    //    //position.x = Input.mousePosition.x+15;
    //    //position.y = Input.mousePosition.y-5;

    //    //transform.position = position;

    //}
}
