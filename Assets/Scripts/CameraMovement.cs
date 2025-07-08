using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed = 10f;       // Speed for panning
    [SerializeField] private float rotationSpeed = 5f;    // Speed for rotating the camera

    [Header("Zoom Settings")]
    [SerializeField] private float minOrthographicSize = 0.01f; // Minimum orthographic size (max zoom)
    [SerializeField] private float maxOrthographicSize = 1f;    // Maximum orthographic size (min zoom)
    [SerializeField] private float defaultOrthographicSize = 0.2f; // Default value for orthographic size
    [SerializeField] private float zoomSpeed = 10f;       // Speed for zooming

    [Header("Target Settings")]
    [SerializeField] private Transform target;            // The point to orbit around
    [SerializeField] private Vector3 initialTargetOffset = Vector3.zero;

    private Camera cameraComponent;                      // Reference to the Camera component
    private Vector3 targetPosition;                      // Current target position
    private Vector2 rotationAngles;

    [Header("Pointer Landmark")]
    [SerializeField] private Transform rightEye;
    [SerializeField] private Transform leftEye;
    [SerializeField] private Transform chin;

    private void Start()
    {
        cameraComponent = GetComponent<Camera>();

        if (cameraComponent == null || !cameraComponent.orthographic)
        {
            Debug.LogError("This script requires an Orthographic Camera.");
            enabled = false;
            return;
        }

        if (target != null)
        {
            targetPosition = target.position + initialTargetOffset;
        }
        else
        {
            targetPosition = transform.position + transform.forward * 10f;
        }

        cameraComponent.orthographicSize = defaultOrthographicSize; // Set default zoom
    }

    private void Update()
    {
        HandleInput();
    }

    private void HandleInput()
    {
        // Rotate camera (Right Mouse Button)
        if (Input.GetMouseButton(1))
        {
            float mouseX = Input.GetAxis("Mouse X");
            float mouseY = Input.GetAxis("Mouse Y");

            rotationAngles.x -= mouseY * rotationSpeed;
            rotationAngles.y += mouseX * rotationSpeed;

            Quaternion rotation = Quaternion.Euler(rotationAngles.x, rotationAngles.y, 0);
            transform.position = targetPosition - rotation * Vector3.forward * 10f; // Keeps a constant distance
            transform.rotation = rotation;
        }

        // Zoom camera (Mouse Scroll Wheel) by changing orthographic size
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if (Mathf.Abs(scroll) > 0.01f)
        {
            cameraComponent.orthographicSize -= scroll * zoomSpeed * Time.deltaTime;
            cameraComponent.orthographicSize = Mathf.Clamp(cameraComponent.orthographicSize, minOrthographicSize, maxOrthographicSize);
        }

        // Pan camera (Middle Mouse Button)
        if (Input.GetMouseButton(2))
        {
            float mouseX = Input.GetAxis("Mouse X");
            float mouseY = Input.GetAxis("Mouse Y");

            Vector3 right = transform.right * -mouseX * moveSpeed * Time.deltaTime;
            Vector3 up = transform.up * -mouseY * moveSpeed * Time.deltaTime;

            targetPosition += right + up;
            transform.position += right + up;
        }
    }

    // Align the camera to face the target from +X axis
    public void AlignToPositiveX()
    {
        AlignCamera(Vector3.right);
    }

    // Align the camera to face the target from -X axis
    public void AlignToNegativeX()
    {
        AlignCamera(Vector3.left);
    }

    // Align the camera to face the target from +Y axis
    public void AlignToPositiveY()
    {
        AlignCamera(Vector3.up);
    }

    // Align the camera to face the target from -Y axis
    public void AlignToNegativeY()
    {
        AlignCamera(Vector3.down);
    }

    // Align the camera to face the target from +Z axis
    public void AlignToPositiveZ()
    {
        AlignCamera(Vector3.forward);
    }

    // Align the camera to face the target from -Z axis
    public void AlignToNegativeZ()
    {
        AlignCamera(Vector3.back);
    }

    // Helper method to align the camera
    private void AlignCamera(Vector3 direction)
    {
        if (target == null)
        {
            Debug.LogWarning("Target is not assigned. Please assign a target in the Inspector.");
            return;
        }

        // Position the camera relative to the target
        transform.position = target.position - direction * 10f; // Maintain a fixed distance

        // Calculate rotation to look at the target
        Quaternion rotation = Quaternion.LookRotation(target.position - transform.position, Vector3.up);
        transform.rotation = rotation;

        // Update the rotation angles
        rotationAngles = new Vector2(transform.eulerAngles.x, transform.eulerAngles.y);
    }

    public void AlignCameraRightEye(){
        targetPosition = rightEye.position;
        transform.position = rightEye.position + (rightEye.position-target.position).normalized * 0.25f;

        Quaternion rotation = Quaternion.LookRotation(rightEye.position - transform.position, Vector3.up);
        transform.rotation = rotation;
        cameraComponent.orthographicSize = 0.05f;

        // Update the rotation angles
        rotationAngles = new Vector2(transform.eulerAngles.x, transform.eulerAngles.y);
    }
    public void AlignCameraLeftEye(){
        targetPosition = leftEye.position;
        transform.position = leftEye.position + (leftEye.position-target.position).normalized * 0.25f;

        Quaternion rotation = Quaternion.LookRotation(leftEye.position - transform.position, Vector3.up);
        transform.rotation = rotation;
        cameraComponent.orthographicSize = 0.05f;

        // Update the rotation angles
        rotationAngles = new Vector2(transform.eulerAngles.x, transform.eulerAngles.y);
    }
    public void AlignCameraChin(){
        targetPosition = chin.position;
        transform.position = chin.position + (chin.position-target.position).normalized * 0.25f;

        Quaternion rotation = Quaternion.LookRotation(chin.position - transform.position, Vector3.up);
        transform.rotation = rotation;
        cameraComponent.orthographicSize = 0.05f;

        // Update the rotation angles
        rotationAngles = new Vector2(transform.eulerAngles.x, transform.eulerAngles.y);
    }
}
