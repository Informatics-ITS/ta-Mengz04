using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GizmoLabelBehaviour : MonoBehaviour
{

    private void LateUpdate()
    {
        Vector3 backward = -Vector3.forward;

        transform.forward = backward;
    }
}
