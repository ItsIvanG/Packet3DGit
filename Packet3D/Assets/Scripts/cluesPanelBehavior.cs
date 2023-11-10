using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class cluesPanelBehavior : MonoBehaviour
{
    Vector2 pos;
    TMP_InputField inputFieldInstance;
    private void Start()
    {
        gameObject.SetActive(false);
        inputFieldInstance = TerminalConsoleBehavior.instance.inputField;
    }
    void Update()
    {
        
        pos.x = inputFieldInstance.transform.position.x+ inputFieldInstance.caretPosition*8;
        pos.y = inputFieldInstance.transform.position.y;
        transform.position = pos;
    }
    
}
