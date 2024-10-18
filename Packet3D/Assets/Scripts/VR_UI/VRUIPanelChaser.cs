using UnityEngine;

public class VRUIPanelChaser : MonoBehaviour
{
    public Transform vrCamera;       // Assign the VR camera (typically the Main Camera) in the inspector
    public float distanceFromCamera = 2.0f; // Desired distance from the camera
    public float chaseThreshold = 60f;  // Angle threshold before the panel starts chasing the camera
    public float chaseSpeed = 2.0f;     // How fast the panel moves to catch up with the camera

    private Vector3 initialOffset;      // The initial position offset from the camera

    void Start()
    {
        if (vrCamera == null)
        {
            vrCamera = Camera.main.transform; // If not assigned, defaults to the main camera
        }
        initialOffset = transform.position - vrCamera.position;
    }

    void Update()
    {
        // Calculate direction and angle between the panel and the camera's forward view
        Vector3 panelToCamera = transform.position - vrCamera.position;
        float angle = Vector3.Angle(vrCamera.forward, panelToCamera);
        Vector3 targetPosition = vrCamera.position + vrCamera.forward * distanceFromCamera;
        targetPosition.y = vrCamera.position.y; // Keep panel at the same vertical level
        transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * chaseSpeed);


        // If the panel is too far off the front view (out of the threshold)
        if (angle > chaseThreshold)
        {
            // Calculate the target position for the panel

            // Smoothly move the panel toward the target position

            // Make the panel face the camera
            transform.LookAt(vrCamera);
            transform.rotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y, 0); // Lock rotation to Y-axis
        }
    }
}