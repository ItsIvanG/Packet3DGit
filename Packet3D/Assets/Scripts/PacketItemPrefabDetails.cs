using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

public class PacketItemPrefabDetails : MonoBehaviour
{
    
    public string type;
    public string Name;
    public Sprite icon;
    TooltipTrigger tooltipTrigger;
    public List<PortProperties> portList = new List<PortProperties>();
    private void Start()
    {
        tooltipTrigger = GetComponent<TooltipTrigger>();
        tooltipTrigger.header = name;
        var ports = GetComponentsInChildren<PortProperties>();
        foreach(var p in ports)
        {
            portList.Add(p);
        }
        UpdateContent();
        
    }
    public void UpdateContent()
    {
        if(tooltipTrigger != null)
        {
            tooltipTrigger.content = "";
        }
        foreach (var p in portList)
        {
            if (p.portHop != null)
            {
                tooltipTrigger.content += p.PortName + " --- " + p.portHop.PortName + " (" + p.portHopParent.name + ")\n";
            }
            else
            {
                tooltipTrigger.content += p.PortName + " --- \n";
            }
        }
        
    }
}
