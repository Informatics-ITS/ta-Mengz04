using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PositionReset : MonoBehaviour
{
    [SerializeField] private Transform targetPos;

    public void ResetPosition() {
        this.transform.position = targetPos.position;
    }
}
