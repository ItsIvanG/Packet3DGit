using UnityEngine;

public class CanvasFollowPlayer : MonoBehaviour
{
    public Transform player;         // The player's camera or head transform
    public float followDistance = 2f;  // Distance in front of the player to place the UI
    public float lerpSpeed = 5f;     // Speed at which the canvas follows the player
    public float angleThreshold = 45f; // Angle beyond which the canvas should reposition

    //public Canvas canvas;           // Reference to the canvas

    void Start()
    {
        player = Camera.main.transform;
    }

    void Update()
    {
        // Get the direction from the player to the canvas
        Vector3 toCanvas = transform.position - player.position;
        toCanvas.y = 0;  // Ensure we are only looking on the XZ plane

        // Get the forward direction of the player
        Vector3 playerForward = player.forward;
        playerForward.y = 0;  // Again, only XZ plane

        // Calculate the angle between the player's forward and the canvas direction
        float angle = Vector3.Angle(playerForward, toCanvas);

        // If the canvas is too far out of view, lerp it back to the front
       
            // Target position is in front of the player at the set distance
            Vector3 targetPosition = player.position + player.forward * followDistance;

            // Lerp the canvas's position smoothly
            transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * lerpSpeed);

            // Rotate the canvas to face the player
           
         Vector3 lookAt = new Vector3(player.position.x, transform.position.y, player.position.z);
            Quaternion targetRotation = Quaternion.LookRotation(transform.position - lookAt);
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * lerpSpeed);
    }
    public void stopFollow()
    {
        lerpSpeed = 0f;
    }
}
