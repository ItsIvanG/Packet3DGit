using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortProperties : MonoBehaviour
{
    public string PortName;
    public PortProperties portHop;
    public GameObject portHopParent;
    public PortTypes.Type PortType;
    public PortTypes.Function PortFunction;
    public GameObject pluggedCable;
    
    public string address;
    public string subnet;
    

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
