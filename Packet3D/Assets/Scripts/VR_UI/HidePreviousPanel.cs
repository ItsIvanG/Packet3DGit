using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HidePreviousPanel : MonoBehaviour
{
    public GameObject hideThis;
    void onEnd()
    {
        hideThis.SetActive(false);
    }
}
