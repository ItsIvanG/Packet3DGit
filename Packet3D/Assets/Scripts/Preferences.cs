using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Preferences : MonoBehaviour
{
    public float canvasScale = 1f;
    // Start is called before the first frame update
    void Awake()
    {
        var canvases = FindObjectsByType<Canvas>(0);
        foreach(var c in canvases)
        {
            c.scaleFactor = canvasScale;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
