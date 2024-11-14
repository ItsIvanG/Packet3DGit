using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trophy : MonoBehaviour
{
    public string LevelsPrefix;
    public int max;
    // Start is called before the first frame update
    void Start()
    {
        int   currentLevel = PlayerPrefs.GetInt("Current" + LevelsPrefix, 0);
        if (currentLevel < max) Destroy(gameObject);

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
