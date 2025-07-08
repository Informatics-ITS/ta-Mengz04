using UnityEngine;

public class HandleDrag : MonoBehaviour
{
    [HideInInspector] public CubeScaler cubeScaler;
    public Vector3 handleDirection;
    private Material normalMat;
    private Material transparentMat;
    private bool isDragging = false;
    private Vector3 dragStartPos;
    private bool enableDrag = true;
    public void SetNormalMaterial(Material mat){
        this.normalMat = mat;
    }
    public void SetTransparentMaterial(Material mat){
        this.transparentMat = mat;
    }
    ///////////////////////////////////////////////////////////////////////////
    public void ToggleVisibility(bool state){
        SetEnableDrag(state);
        MeshRenderer renderer = GetComponent<MeshRenderer>();
        if (renderer != null) renderer.material = (state)? normalMat : transparentMat;
    }
    public void SetEnableDrag(bool state){
        this.enableDrag = state;
    }

    void OnMouseDown()
    {
        if(!enableDrag) return;
        isDragging = true;
        dragStartPos = transform.position;
    }

    void OnMouseUp()
    {
        if(!enableDrag) return;
        isDragging = false;
    }

    void Update()
    {
        if (isDragging)
        {
            Vector3 mousePosition = Input.mousePosition;
            mousePosition.z = Camera.main.WorldToScreenPoint(transform.position).z;
            Vector3 worldPos = Camera.main.ScreenToWorldPoint(mousePosition);

            Vector3 delta = worldPos - dragStartPos; // Change in position
            cubeScaler.ScaleCube(transform, delta, handleDirection);
            dragStartPos = worldPos;
        }
    }
}
