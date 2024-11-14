using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowroomPodium : MonoBehaviour
{
    public float spinRate;
    public float lerpRate=10f;
    [SerializeField]
    private float spinFloat = 180f;
   
    private void Update()
    {
        spinFloat += spinRate * Time.deltaTime;
        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0, spinFloat,0), lerpRate);
    }
    public void spin(float r)
    {
        spinRate = r;

    }
    public void stopSpin()
    {
        spinRate = 0;
    }
}
