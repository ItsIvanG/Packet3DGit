using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnvironmentManager : MonoBehaviour
{
    public GameObject[] Envis;
    public static EnvironmentManager instance;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != null && instance != this)
        {
            Destroy(this);
        }


    }
    void Start()
    {
        if(transform.parent != null)
        {
            transform.parent = null;//escape the sci-fi root
        }
        if (PlayerPrefs.GetInt("Envi") > 0)
        {
            Destroy(GameObject.FindGameObjectWithTag("Envi"));
            Instantiate(Envis[PlayerPrefs.GetInt("Envi")]);
        }
    }

    public void refreshEnvi()
    {
        Destroy(GameObject.FindGameObjectWithTag("Envi"));
        Instantiate(Envis[PlayerPrefs.GetInt("Envi")]);
    }

}
