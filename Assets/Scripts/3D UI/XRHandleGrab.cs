using UnityEngine;
using Oculus.Interaction;

public class XRHandleDrag : MonoBehaviour
{
    [HideInInspector] public XRCubeScaler xrCubeScaler;
    [HideInInspector] public Vector3 handleDirection;

    private bool isDragging;
    private Vector3 dragStartPos;
    private bool enableDrag = true;

    public void SetEnableDrag(bool state)
    {
        this.enableDrag = state;
    }

    public void OnPinch()
    {
        if (!enableDrag) return;
        isDragging = true;
        dragStartPos = transform.position;
    }

    public void OnRelease()
    {
        if (!enableDrag) return;
        isDragging = false;
    }

    void Update()
    {
        if (isDragging)
        {
            Vector3 delta = transform.position - dragStartPos; // Change in position
            xrCubeScaler.ScaleCube(transform, delta, handleDirection);
            dragStartPos = transform.position;
        }
    }
}
