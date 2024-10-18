using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TooltipSystem : MonoBehaviour
{
    private static TooltipSystem current;
    public Tooltip tooltip;
    public GameObject rightControl;
    TooltipTrigger lastTooltip = null;
    bool flashedTooltip = false;
    public float adjustForward = 0.001f;
    LayerMask rayMask;
    public void Awake()
    {
        current = this;
        // Define the layer indices you want to exclude
        int[] exclusionLayerIndices = {2,7 };  // ALSO CHANGE IN TOOLTIP.CS IF CHANGING

        // Initialize LayerMask to include all layers
        rayMask = ~0; // This means all layers

        foreach (int layerIndex in exclusionLayerIndices)
        {
            // Exclude the layer by using bitwise AND with the inverted layer
            rayMask &= ~(1 << layerIndex);
        }
    }

    public static void Show( string header, string content, string sub)
    {
        current.tooltip.SetText(header, content, sub);
        current.tooltip.gameObject.SetActive(true);
    }
    public static void Hide()
    {
        current.tooltip.gameObject.SetActive(false);
    }
    private void FixedUpdate()
    {
        RaycastHit hit;
        bool RightRay = Physics.Raycast(rightControl.transform.position, rightControl.transform.forward, out hit, 100, rayMask);

        TooltipTrigger hasTooltip = null;
        if (RightRay) hasTooltip = hit.collider.transform.gameObject.GetComponent<TooltipTrigger>();

        if (RightRay)
        {

            //Debug.Log("TOOLTIP @ " + hit.collider.name);

            if (hasTooltip && (lastTooltip!=hasTooltip))
            {
                hasTooltip.onRayEnter();
                lastTooltip = hasTooltip;
                flashedTooltip = true;

                
                tooltip.transform.rotation = hit.collider.transform.rotation;
                tooltip.transform.forward *= -1;
                tooltip.transform.position = hit.collider.transform.position - (tooltip.transform.forward* adjustForward);
            }
            else if (!hasTooltip && lastTooltip)
            {
                lastTooltip.onRayLeave();
                lastTooltip = null;
                flashedTooltip = false;
            }

        }
       
    }
}
