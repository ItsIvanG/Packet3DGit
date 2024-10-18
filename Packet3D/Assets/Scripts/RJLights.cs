using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VInspector;

[ExecuteInEditMode]
public class RJLights : MonoBehaviour
{
    public GameObject lightParent;
    //public lightType lights;
    //public enum lightType
    //{
    //    None,Square,Triangle
    //}


    [Button("Spawn Square lights")]
    public void SqrLights()
    {
        deleteLights();

        GameObject load = Resources.Load<GameObject>("Prefabs/RJ_LIGHT_SQR");

        Instantiate(load, lightParent.transform);
    }

    [Button("Spawn Triangle lights")]
    public void TriLights()
    {
        deleteLights();

        GameObject load = Resources.Load<GameObject>("Prefabs/RJ_LIGHT_TRI");

        Instantiate(load, lightParent.transform);
    }
    [Button("Delete lights")]
    public void deleteLights()
    {
        foreach (Transform child in lightParent.transform)
        {
            DestroyImmediate(child.gameObject);
        }
    }

    //private void OnValidate()
    //{
    //    switch (lights)
    //    {
    //        case lightType.Square:
    //            SqrLights();
    //            break;
    //        case lightType.Triangle:
    //            TriLights();
    //            break;
    //        case lightType.None:
    //            deleteLights();
    //            break;
    //    }
    //}
}
