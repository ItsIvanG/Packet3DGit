using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClearPrefs : MonoBehaviour
{
   public void clear()
    {
        PlayerPrefs.DeleteAll();
    }
}
