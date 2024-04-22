using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.UI;
using UnityEngine.XR.Management;

public class packetItemButtonFunc : MonoBehaviour, IPointerEnterHandler
{
    private XRUIInputModule InputModule => EventSystem.current.currentInputModule as XRUIInputModule;

    AddManager addManager;
    public Button button;
    public PacketItem packetItem;
    XRRayInteractor interactor;
    // Start is called before the first frame update
    void Start()
    {
        addManager=FindAnyObjectByType<AddManager>();

    }


    public void setAddMngrItem()
    {
        Debug.Log("clicked in packet item: " + packetItem);
        addManager.setPacketItem(packetItem, interactor.gameObject);
        FindAnyObjectByType<SelectTransform>().setTransformMode(0);
        addManager.disableAllCableCollisions();

    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        interactor = InputModule.GetInteractor(eventData.pointerId) as XRRayInteractor;

    }
}
