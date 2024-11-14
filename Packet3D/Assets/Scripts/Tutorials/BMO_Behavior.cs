using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VInspector;

public class BMO_Behavior : MonoBehaviour
{
    public Animator animator;
    public Transform player;    // Reference to the player's transform
    public float rotationSpeed = 5f;  // Speed of the rotation
    public float moveSpeed = 5f;  // Speed of the rotation
    public float camMargin = -1.9f;
    public float bmoMargin = -3.4f;
    public float bmoDefaultX = -1.577f;
    private Vector3 targetPos;
    public AudioClip[] bmoBlips;
    public AudioSource bmoAudioSource;
    void Update()
    {
        // Get the direction to the player
        Vector3 direction = player.position - transform.position;

        // Zero out the y-axis to ensure the Z-up orientation
        direction.y = 0;

        // Check if direction has magnitude to avoid errors when player and object are at the same position
        if (direction.magnitude > 0.1f)
        {
            // Get the target rotation based on the direction with Z-up orientation
            Quaternion targetRotation = Quaternion.LookRotation(direction, Vector3.up);

            // Lerp the current rotation to the target rotation
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);
        }

        if(player.position.x < camMargin)
        {
            targetPos.x = bmoMargin;
        }
        else
        {
            targetPos.x = bmoDefaultX;
        }
        transform.position = Vector3.Lerp(transform.position, targetPos, moveSpeed * Time.deltaTime);
    }
    void Awake()
    {
        animator = GetComponentInChildren<Animator>();
        player = Camera.main.transform;
        targetPos = transform.position;

    }
    [Button("Wait Done")]
    public void WaitDone()
    {
        int r = Random.Range(0, 2);
        switch (r)
        {
            case 0:
                animator.SetTrigger("spin");
                break;
            case 1:

                animator.SetTrigger("flip");
                break;
        }

    }
    [Button("Dance")]
    public void Dance()
    {
        animator.SetTrigger("dance");
    }
    [Button("Yap")]
    public void Yap()
    {
        Debug.Log(" yap");

        animator.SetTrigger("yap");
    }
    [Button("Stop Yap")]
    public void StopYap()
    {
        Debug.Log("Stop yap");
        animator.SetTrigger("stop yap");

    }
    public void playBlip()
    {
        bmoAudioSource.clip = bmoBlips[Random.Range(0,bmoBlips.Length-1)];
        bmoAudioSource.Play();
    }

    
}
