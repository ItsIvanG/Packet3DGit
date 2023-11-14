using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PopupMessage : MonoBehaviour
{
    public static PopupMessage instance;
    public enum MsgType { Info, Error };
    public GameObject panel;
    public TextMeshProUGUI titleString;
    public TextMeshProUGUI contentString;
    public Image messageIcon;
    [Header("Icons to use")]
    public Sprite warnIcon;
    public Sprite InfoIcon;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    public static void showMessage(string title, string content, MsgType type)
    {
        instance.panel.SetActive(true);
        switch (type)
        {
            case MsgType.Info:
                instance.messageIcon.sprite = instance.InfoIcon;
                break;
            case MsgType.Error:
                instance.messageIcon.sprite = instance.warnIcon;
                break;
        }
        instance.titleString.text = title;
        instance.contentString.text = content;
    }

}
