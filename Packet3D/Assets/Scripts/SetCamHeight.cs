using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SetCamHeight : MonoBehaviour
{
    public Slider heightSlider;
   public void setHeight()
    {
        PlayerPrefs.SetFloat("Height", heightSlider.value);
        transform.localPosition = new Vector3(0, heightSlider.value, 0);
    }
    private void Start()
    {
        transform.localPosition = new Vector3(0, PlayerPrefs.GetFloat("Height",1.1176f), 0);
    }
}
