using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SuccessSFX : MonoBehaviour
{
    public AudioSource audioSource;
    public void playSuccess()
    {
        audioSource.Play();
    }
}
