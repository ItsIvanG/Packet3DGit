using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TooltipTrigger : MonoBehaviour
{
    public string subtitle;
    public string header;
    [TextArea]
    public string content;
    public void onRayEnter()
    {
        //Debug.Log("Showing tooltip " + header);
        TooltipSystem.Show(header, content, subtitle);
    }
    public void onRayLeave()
    {
        TooltipSystem.Hide();
    }

    public void OnMouseOver()
    {

        if (!EventSystem.current.IsPointerOverGameObject())
        {
            TooltipSystem.Show(header, content, subtitle);
        }
        else
        {
            TooltipSystem.Hide();
        }
    }

    public void OnMouseExit()
    {
        TooltipSystem.Hide();
    }

    
}
