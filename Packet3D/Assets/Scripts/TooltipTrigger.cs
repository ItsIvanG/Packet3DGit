using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TooltipTrigger : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public string subtitle;
    public string header;
    [TextArea]
    public string content;
    public void OnPointerEnter(PointerEventData eventData)
    {
        TooltipSystem.Show(header, content, subtitle);
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        TooltipSystem.Hide();
    }

    public void OnMouseEnter()
    {
        TooltipSystem.Show(header, content, subtitle);

    }

    public void OnMouseExit()
    {
        TooltipSystem.Hide();
    }
}
