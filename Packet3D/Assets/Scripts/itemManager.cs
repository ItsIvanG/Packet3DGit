using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class itemManager : MonoBehaviour
{
    public Transform itemContent;
    public GameObject itemButton;
    public List<PacketItem> items = new List<PacketItem>();
    public List<Button> buttons = new List<Button>();
    public GameObject itemsCategoryPanel;
    //public List<PacketItem> itemsSwitches = new List<PacketItem>();
    //public List<PacketItem> itemsEndDevices = new List<PacketItem>();


    // Start is called before the first frame update
    void Awake()
    {
        var a = Resources.LoadAll<PacketItem>("PacketItems");
        foreach(var item in a)
        {
            items.Add(item);
        }

        var getButtons = itemsCategoryPanel.GetComponentsInChildren<Button>();
        foreach(var b in getButtons)
        {
            buttons.Add(b);
        }
     
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void loadToScreen(string type)
    {
        Debug.Log("Load to screen:" + type);
        foreach(Transform i in itemContent)
        {
            Destroy(i.gameObject);
        }
        //LOAD to UI
        foreach (var i in items)
        {
            if (i.type.ToString() == type)
            {
                GameObject obj = Instantiate(itemButton, itemContent);
                var itemName = obj.transform.Find("Name").GetComponent<TextMeshProUGUI>();
                var itemIcon = obj.transform.Find("Image").GetComponent<Image>();
                var itemFunc = obj.transform.GetComponent<packetItemButtonFunc>();
                var itemTooltip = obj.transform.GetComponent<TooltipTrigger>();

                itemName.text = i.ShortName;
                itemIcon.sprite = i.Thumbnail;
                itemFunc.packetItem = i;
                itemTooltip.header = i.Name;
                itemTooltip.content = i.Description;
                Debug.Log("opened " + (int)i.type + " list");

                //SET ITEM CATEGORY ACTIVE COLOR
                buttons[(int)i.type].GetComponent<Image>().color = UIColorManagerScript.ButtonActiveColor;

                //SET OTHER ITEM CATEGORIES IDLE COLOR
                for (int z = 0; z < buttons.Count; z++)
                {
                    if (z != (int)i.type)
                    {
                        buttons[z].GetComponent<Image>().color = UIColorManagerScript.ButtonIdleColor;
                    }
                }
            }
        }
        

    }
}
