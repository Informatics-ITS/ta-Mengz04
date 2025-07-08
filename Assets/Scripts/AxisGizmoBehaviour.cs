using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AxisGizmoBehaviour : MonoBehaviour
{
    [SerializeField] private Camera targetCamera;  // The camera whose rotation to negate

    private void Update()
    {
        if (targetCamera == null)
        {
            Debug.LogWarning("Please assign both Target Camera and Target Object.");
            return;
        }

        // Get the camera's rotation
        Quaternion cameraRotation = targetCamera.transform.rotation;

        // Negate the rotation
        Quaternion negatedRotation = Quaternion.Inverse(cameraRotation);

        // Apply the negated rotation to the target object
        this.transform.rotation = negatedRotation;
    }
}
