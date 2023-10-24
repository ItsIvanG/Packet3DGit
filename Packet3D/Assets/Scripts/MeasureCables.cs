using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class MeasureCables : MonoBehaviour
{
    public TMP_Dropdown dropdown;
    public void DoMeasure()
    {
        //0: disabled
        //1: cm
        //2: m
        //3: in
        //4: ft
        Debug.Log("Measure: " + dropdown.value);
        var cableRenders = FindObjectsByType<cableRender>(0);
        foreach(var cable in cableRenders)
        {
            Debug.Log("Found cable: " + cable);
            
            cable.Measure();
        }
    }
}
