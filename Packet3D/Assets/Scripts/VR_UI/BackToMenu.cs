using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class BackToMenu : MonoBehaviour
{
    public InputActionProperty yButtonAction;
    void Update()
    {
        if (yButtonAction.action.WasPressedThisFrame())
        {
            UIFadeInAndLoadScene uif = FindAnyObjectByType<UIFadeInAndLoadScene>();
            uif.StartFade();
        }
    }
}
