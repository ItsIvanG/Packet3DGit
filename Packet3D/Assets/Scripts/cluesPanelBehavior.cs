using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class cluesPanelBehavior : MonoBehaviour
{
    Vector3 pos;
    Quaternion rot;
    TMP_InputField inputFieldInstance;
    private void Start()
    {
        gameObject.SetActive(false);
        inputFieldInstance = TerminalConsoleBehavior.instance.inputField;
    }
    void Update()
    {

        pos = DesktopCanvasScript.instance.currentPC.transform.Find("KB Clues").position;
        rot = DesktopCanvasScript.instance.currentPC.transform.Find("KB Clues").rotation;
        transform.position = pos;
        transform.rotation = rot;
    }
    
}
