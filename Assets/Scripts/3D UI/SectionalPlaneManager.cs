using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SectionalPlaneManager : MonoBehaviour
{
    [SerializeField] private GameObject dispPlane;

    public void RotateClockwise() {
        dispPlane.transform.rotation *= Quaternion.Euler(0f, 90f, 0f);
    }

	public void RotateCounterClockwise()
	{
		dispPlane.transform.rotation *= Quaternion.Euler(0f, -90f, 0f);
	}
}
