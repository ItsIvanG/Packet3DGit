using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class cluesPanelBehavior : MonoBehaviour
{
    Vector3 pos;
    TMP_InputField inputFieldInstance;
    private void Start()
    {
        gameObject.SetActive(false);
        inputFieldInstance = TerminalConsoleBehavior.instance.inputField;
    }
    void Update()
    {
        
        pos.x = inputFieldInstance.transform.position.x+ inputFieldInstance.caretPosition* 0.0040125392f; //8f at 1x
        pos.y = inputFieldInstance.transform.position.y;
        pos.z = inputFieldInstance.transform.position.z;
        transform.position = pos;
    }
    
}
