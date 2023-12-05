using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class packetItemButtonFunc : MonoBehaviour
{
    AddManager addManager;
    public Button button;
    public PacketItem packetItem;
    // Start is called before the first frame update
    void Start()
    {
        addManager=FindAnyObjectByType<AddManager>();

    }


    public void setAddMngrItem()
    {
        Debug.Log("clicked in packet item: " + packetItem);
        addManager.setPacketItem(packetItem);
        FindAnyObjectByType<SelectTransform>().setTransformMode(0);
        addManager.disableAllCableCollisions();
    }
}
