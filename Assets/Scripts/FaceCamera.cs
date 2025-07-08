using UnityEngine;

public class FaceCamera : MonoBehaviour
{
    public Camera targetCamera; // Assign the target camera in the inspector

    void Start()
    {
        // If no camera is assigned, use the main camera
        if (targetCamera == null)
        {
            targetCamera = Camera.main;
        }
    }

    void Update()
    {
        if (targetCamera != null)
        {
            // Make the GameObject face the camera
            transform.rotation = Quaternion.LookRotation(transform.position - targetCamera.transform.position);
        }
    }
}
