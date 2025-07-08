using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FacingUpOrientation : MonoBehaviour
{
    void LateUpdate()
    {

        Vector3 euler = transform.rotation.eulerAngles;

        euler.z = 0f;

        transform.rotation = Quaternion.Euler(euler);

    }
}
