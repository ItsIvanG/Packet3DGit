using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using VInspector;

public class ShowroomManager : MonoBehaviour
{
    public List<PacketItem> items;
    public GameObject itemPrefab;
    public Transform addItemsHere;
    public TextMeshProUGUI specificName, specificDescription, chooseItemHeader;
    public Image specificThumbnail;
    public GameObject specificPanel;
    public Transform spawnHere;
    void Start()
    {
        var a = Resources.LoadAll<PacketItem>("PacketItems");
        foreach (var item in a)
        {
            items.Add(item);
        }

    }

    public void showItems(string type)
    {
        foreach(Transform t in addItemsHere)
        {
            Destroy(t.gameObject);
        }
        foreach(var i in items)
        {
            if (i.type.ToString() == type)
            {
                GameObject p = Instantiate(itemPrefab, addItemsHere);
                p.transform.Find("Image").GetComponent<Image>().sprite = i.Thumbnail;
                p.GetComponentInChildren<TextMeshProUGUI>().text = i.Name;
                p.GetComponent<ShowroomItem>().item = i;
            }
        }
        chooseItemHeader.text = "Choose " + type;
    }

    public void showSpecific(PacketItem item)
    {
        specificName.text = item.Name;
        specificDescription.text = item.Description;
        specificThumbnail.sprite = item.Thumbnail;
        specificPanel.SetActive(true);

        foreach (Transform t in spawnHere)
        {
            Destroy(t.gameObject);
        }

        if (item.ShowroomObject)
        {
            Instantiate(item.ShowroomObject, spawnHere);
        }
        else
        {
            Instantiate(item.GameObject, spawnHere);
        }
    }

    [Button("Show Routers")]
    public void showRouters()
    {
        showItems("Router");
    }
    [Button("Show Switches")]
    public void showSwitch()
    {
        showItems("Switch");
    }
    [Button("Show Cables")]
    public void showCables()
    {
        showItems("Cable");
    }
    [Button("Show End Devices")]
    public void showEndDevices()
    {
        showItems("EndDevice");
    }




}
