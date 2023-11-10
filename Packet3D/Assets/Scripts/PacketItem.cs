using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Packet Item",menuName = "Packet3D/Create Packet Item")]

public class PacketItem : ScriptableObject
{
    public enum Type {Router, Switch, EndDevice, Misc, Cable };
    public Type type;
    public string Name;
    public string ShortName;
    public string Description;
    public Sprite Thumbnail;
    public GameObject GameObject;

    public void Awake()
    {
        if (GameObject != null && Name!=null)
        {
            PacketItemPrefabDetails details = GameObject.GetComponent<PacketItemPrefabDetails>();
            if (details != null)
            {
                details.Name = Name;
                details.type = type;
            }
            
            TooltipTrigger trigger = GameObject.GetComponent<TooltipTrigger>();

            if (trigger!=null)
            {
                trigger.subtitle = type.ToString();
                
            }
            
        }
        
    }

}
