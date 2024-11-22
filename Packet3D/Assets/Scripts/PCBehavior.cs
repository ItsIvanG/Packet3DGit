using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PCBehavior : MonoBehaviour
{
    [TextArea]
    public string terminalContent;
    public enum CurrentMenu
    {
        Desktop,
        IPConfig,
        CMD,
        Terminal,
        Putty,
        DeviceManager
    }
    public CurrentMenu currentMenu = CurrentMenu.Desktop;
    public string successPing,successTelnet;
}
