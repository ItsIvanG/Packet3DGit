using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Microsoft.MixedReality.Toolkit.Experimental.UI;

public class TerminalCanvasScript : MonoBehaviour
{
    public static TerminalCanvasScript instance;
    public GameObject currentDevice;
    public TextMeshProUGUI deviceLabel;
    public TextMeshProUGUI hostnameLabel;
    public TMP_InputField inputField;
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
        instance.inputField.text = "";
        instance. SetKB();
        instance.gameObject.SetActive(true);
        instance.currentDevice = device;
        instance.deviceLabel.text = device.name;
        
        
        
        TerminalConsoleBehavior.instance.currentObj = device;
        TerminalConsoleBehavior.instance.getVarsFromCisco();
        TerminalLogin.checkLogin();
        instance.updateHostnamePrefix();
    }
    public void SetKB()
    {
        instance.inputField.onSelect.AddListener(x => instance.OpenKeyboard());
    }
    public void OpenKeyboard()
    {
        NonNativeKeyboard.Instance.InputField = inputField;
        NonNativeKeyboard.Instance.PresentKeyboard(inputField.text);
    }

    public void updateHostnamePrefix()
    {
        if(TerminalConsoleBehavior.instance.currentPrivilege > TerminalPrivileges.privileges.loggedOut)
        {
            if (instance.currentDevice.GetComponent<CiscoDevice>())
            {
                instance.hostnameLabel.text = instance.currentDevice.GetComponent<CiscoDevice>().hostname + TerminalConsoleBehavior.instance.getPrivilegePrefix();
                TerminalConsoleBehavior.instance.saveVarsToCisco();
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
