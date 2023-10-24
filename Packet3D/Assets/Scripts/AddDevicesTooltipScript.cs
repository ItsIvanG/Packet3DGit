using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class AddDevicesTooltipScript : MonoBehaviour
{
    private TextMeshProUGUI tmp;
    // Start is called before the first frame update
    void Start()
    {
        tmp = GetComponentInChildren<TextMeshProUGUI>();
    }

    public void setToolTip(string text)
    {
        tmp.text = text;
        this.gameObject.SetActive(true);
    }
}
