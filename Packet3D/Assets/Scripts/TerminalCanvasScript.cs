using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TerminalCanvasScript : MonoBehaviour
{
    public static TerminalCanvasScript instance;
    public GameObject currentDevice;
    public TextMeshProUGUI deviceLabel;

    private void Awake()
    {
        instance = this;
        instance.gameObject.SetActive(false);
    }

    public static void ShowTerminal(GameObject device)
    {
        instance.gameObject.SetActive(true);
        instance.currentDevice = device;
        instance.deviceLabel.text = device.name;
    }
}
