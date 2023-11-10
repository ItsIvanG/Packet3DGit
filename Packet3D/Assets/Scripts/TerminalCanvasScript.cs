using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TerminalCanvasScript : MonoBehaviour
{
    public static TerminalCanvasScript instance;
    public GameObject currentDevice;
    public TextMeshProUGUI deviceLabel;
    public TextMeshProUGUI hostnameLabel;
    public bool updateHostnamePrefixNow;
    private void Awake()
    {
        instance = this;
        instance.gameObject.SetActive(false);
    }

    private void Update() //DEBUG PURPOSES
    {
        if (updateHostnamePrefixNow)
        {
            updateHostnamePrefix();
            updateHostnamePrefixNow = !updateHostnamePrefixNow;
        }
    }

    public static void ShowTerminal(GameObject device)
    {
        instance.gameObject.SetActive(true);
        instance.currentDevice = device;
        instance.deviceLabel.text = device.name;
        TerminalLogin.checkLogin();
        instance.updateHostnamePrefix();
        TerminalConsoleBehavior.instance.currentObj = device;
    }

    public void updateHostnamePrefix()
    {
        if(TerminalConsoleBehavior.instance.currentPrivilege > TerminalPrivileges.privileges.loggedOut)
        {
            if (instance.currentDevice.GetComponent<RouterProperties>())
            {
                instance.hostnameLabel.text = instance.currentDevice.GetComponent<RouterProperties>().hostname + TerminalConsoleBehavior.instance.getPrivilegePrefix();
            }
            else if (instance.currentDevice.GetComponent<SwitchProperties>())
            {
                instance.hostnameLabel.text = instance.currentDevice.GetComponent<SwitchProperties>().hostname + TerminalConsoleBehavior.instance.getPrivilegePrefix();
            }
        }
        else
        {
            switch (TerminalConsoleBehavior.instance.enteringLocalWhat)
            {
                case 0:
                    instance.hostnameLabel.text = "Username:";
                    break;
                case 1:
                    instance.hostnameLabel.text = "Password:";
                    break;
                case 2:
                    instance.hostnameLabel.text = "Press RETURN to get started.";
                    break;

            }
        }
       
    }
}
