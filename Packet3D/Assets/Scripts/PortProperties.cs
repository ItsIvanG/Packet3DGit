using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortProperties : MonoBehaviour
{
    public enum Type { RJ45, USB};
    public string PortName;
    public PortProperties portHop;
    public GameObject portHopParent;
    public Type PortType;
    private TooltipTrigger tooltipTrigger;
    // Start is called before the first frame update
    void Awake()
    {
        
        tooltipTrigger = GetComponent<TooltipTrigger>();
        tooltipTrigger.subtitle = "PORT: " + PortType.ToString();
        tooltipTrigger.header = PortName;
        
    }
    private void Update()
    {
        if(portHop != null)
        {
            tooltipTrigger.content = portHop.PortName;
        }
    }


}
