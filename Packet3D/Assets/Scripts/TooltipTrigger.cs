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
