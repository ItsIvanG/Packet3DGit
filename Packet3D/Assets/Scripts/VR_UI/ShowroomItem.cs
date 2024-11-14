using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VInspector;

public class ShowroomItem : MonoBehaviour
{
    public PacketItem item;
    [Button("Show")]
    public void showSpecific()
    {
        FindAnyObjectByType<ShowroomManager>().showSpecific(item);
    }
}
