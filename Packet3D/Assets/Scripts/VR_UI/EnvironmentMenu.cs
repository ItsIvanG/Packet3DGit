using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnvironmentMenu : MonoBehaviour
{
    public int environmentIndex;
    public Button chooseButton;

    public void choose()
    {
        PlayerPrefs.SetInt("Envi", environmentIndex);
        EnvironmentManager.instance.refreshEnvi();
    }
}
