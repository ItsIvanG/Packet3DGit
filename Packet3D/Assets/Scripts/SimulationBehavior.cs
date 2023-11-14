using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class SimulationBehavior : MonoBehaviour
{
    public static SimulationBehavior instance;
    private GameObject tempApick;
    private GameObject tempBpick;
    public GameObject APick;
    public GameObject BPick;
    public TMP_Dropdown A_Dropdown;
    public TMP_Dropdown B_Dropdown;
    public List<GameObject> DevicesList;
    public List<string> DevicesNames;
    public Toggle speed0_5x;
    public Toggle speed1x;
    public Toggle speed2x;
    public bool isPicking;
    public int pickingWhat = 0;//0 - FROM. 1 - TO
    public float SimSpeed =1f;
    public List<GameObject> hopsFound;

    private void Awake()
    {
        instance = this;
        refreshList();
    }
    public void refreshList()
    {
        if (instance.DevicesList.Count>0)
        {
            instance.tempApick = instance.DevicesList[instance.A_Dropdown.value];
            instance.tempBpick = instance.DevicesList[instance.B_Dropdown.value];
        }
        instance.A_Dropdown.ClearOptions();
        instance.B_Dropdown.ClearOptions();
        instance.DevicesList.Clear();
        instance.DevicesNames.Clear();
        var DevicesFound = FindObjectsByType<PacketItemPrefabDetails>(0);
        foreach(PacketItemPrefabDetails d in DevicesFound)
        {
            instance.DevicesList.Add(d.gameObject);
            instance.DevicesNames.Add(d.name);
        }
        instance.A_Dropdown.AddOptions(instance.DevicesNames);
        instance.B_Dropdown.AddOptions(instance.DevicesNames);
        instance.A_Dropdown.value = instance.DevicesList.IndexOf(instance.tempApick);
        instance.B_Dropdown.value = instance.DevicesList.IndexOf(instance.tempBpick);
    }

    public void StartPickObject(int what)
    {
        pickingWhat = what;
        isPicking = true;

        if(what == 0)
        {
            APick.GetComponent<Image>().color = UIColorManagerScript.ButtonActiveColor;
            BPick.GetComponent<Image>().color = UIColorManagerScript.ButtonIdleColor;
        }
        else
        {
            BPick.GetComponent<Image>().color = UIColorManagerScript.ButtonActiveColor;
            APick.GetComponent<Image>().color = UIColorManagerScript.ButtonIdleColor;
        }

    }
    public void CancelPick()
    {
        isPicking = false;
        APick.GetComponent<Image>().color = UIColorManagerScript.ButtonIdleColor;
        BPick.GetComponent<Image>().color = UIColorManagerScript.ButtonIdleColor;
    }

    public void pickedObj(GameObject obj)
    {
        CancelPick();
        Debug.Log("Picked object " + obj);
        switch (pickingWhat)
        {
            case 0:
                A_Dropdown.value = DevicesList.IndexOf(obj);
                Debug.Log("Set A dropdown to " + obj);
                break;

            case 1:
                B_Dropdown.value = DevicesList.IndexOf(obj);
                Debug.Log("Set B dropdown to " + obj);
                break;
        }
    }
    public static void refreshDelay()
    {
        instance.Invoke("refreshList", 0.1f);
    }
    public void playSim()
    {
        Debug.Log("doing recursive test: " + DevicesList[A_Dropdown.value]);
        hopsFound.Add(DevicesList[A_Dropdown.value]);
        recursiveTest(DevicesList[A_Dropdown.value]);
    }

    public void recursiveTest(GameObject g)
    {
        Debug.Log("current recursive: " +g);
        var ports = g.GetComponentsInChildren<PortProperties>();
        foreach(var p in ports)
        {

            if (p.portHopParent != null && !hopsFound.Contains(p.portHopParent))
            {
                Debug.Log("RECURSIVE FOUND: " + p.portHopParent);
                hopsFound.Add(p.portHopParent);
                recursiveTest(p.portHopParent);

            }
        }
    }

    public void setAnimSpeed(float speed)
    {
        SimSpeed = speed;
    }
}
