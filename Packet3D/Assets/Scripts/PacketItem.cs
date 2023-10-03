using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Packet Item",menuName = "Packet3D/Create Packet Item")]

public class PacketItem : ScriptableObject
{
    public enum Type {Router, Switch, EndDevice, Misc };
    public Type type;
    public string Name;
    public string ShortName;
    public string Description;
    public Sprite Thumbnail;
    public GameObject GameObject;

}
