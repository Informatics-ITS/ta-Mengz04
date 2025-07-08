using UnityEngine;
using Oculus.Interaction;

public class XRCubeScaler : MonoBehaviour
{
    public Transform cube; // The cube to be scaled
    private Transform[] handles = new Transform[6]; // 6 handles
    [SerializeField] private GameObject handlePrefab;
    private Vector3[] handleDirections = new Vector3[6];
    [SerializeField] private Material handleMat;
    private Vector3 initialCubeSize;
    void Start()
    {
        if (cube == null)
        {
            Debug.LogError("CubeScaler: Assign a cube.");
            return;
        }
        initialCubeSize = cube.localScale;
        CreateHandles();
    }

    void CreateHandles()
    {
        handleDirections = new Vector3[]{
            Vector3.right, Vector3.left, // X+ and X-
            Vector3.up, Vector3.down,   // Y+ and Y-
            Vector3.forward, Vector3.back // Z+ and Z-
        };

        for (int i = 0; i < 6; i++)
        {
            GameObject handle = Instantiate(handlePrefab, transform.position, Quaternion.identity);
            handle.transform.SetParent(transform);

            // Position handle at correct face
            handle.transform.position = cube.position + (Vector3.Scale(cube.lossyScale, handleDirections[i]) / 2);
            //handle.transform.position = cube.position + (cube.TransformDirection(handleDirections[i]) * cube.lossyScale.magnitude / 2);
            
            MeshRenderer renderer = handle.GetComponent<MeshRenderer>();
            renderer.material = handleMat;

            // Add a dragging script
            XRHandleDrag dragScript = handle.GetComponent<XRHandleDrag>();
            dragScript.xrCubeScaler = this;
            dragScript.handleDirection = handleDirections[i]; // Set movement direction

            ScalerOneGrabTranslateTransformer sogtt = handle.GetComponent<ScalerOneGrabTranslateTransformer>();
            ScalerOneGrabTranslateTransformer.OneGrabTranslateConstraints ogtc = new ScalerOneGrabTranslateTransformer.OneGrabTranslateConstraints()
            {
                ConstraintsAreRelative = true,
                MinX = new FloatConstraint(),
                MaxX = new FloatConstraint(),
                MinY = new FloatConstraint(),
                MaxY = new FloatConstraint(),
                MinZ = new FloatConstraint(),
                MaxZ = new FloatConstraint()
            };

            if(handleDirections[i] == Vector3.up || handleDirections[i] == Vector3.down){
                ogtc.MinX.Constrain = true;
                ogtc.MaxX.Constrain = true;

                ogtc.MinY.Constrain = false;
                ogtc.MaxY.Constrain = false;

                ogtc.MinZ.Constrain = true;
                ogtc.MaxZ.Constrain = true;
            }else if(handleDirections[i] == Vector3.right || handleDirections[i] == Vector3.left){
                ogtc.MinX.Constrain = false;
                ogtc.MaxX.Constrain = false;

                ogtc.MinY.Constrain = true;
                ogtc.MaxY.Constrain = true;

                ogtc.MinZ.Constrain = true;
                ogtc.MaxZ.Constrain = true;
            }else if(handleDirections[i] == Vector3.forward || handleDirections[i] == Vector3.back){
                ogtc.MinX.Constrain = true;
                ogtc.MaxX.Constrain = true;

                ogtc.MinY.Constrain = true;
                ogtc.MaxY.Constrain = true;

                ogtc.MinZ.Constrain = false;
                ogtc.MaxZ.Constrain = false;
            }
            else{
                Debug.LogError("Handle directions out of bound!");
            }

            sogtt.Constraints = ogtc;

            // Store handle reference
            handles[i] = handle.transform;
        }
    }

    public void SetVisibility(bool state){
        foreach(Transform handle in handles){
            handle.GetComponent<MeshRenderer>().enabled = state;
            handle.GetComponent<XRHandleDrag>().SetEnableDrag(state);
        }
        cube.gameObject.GetComponent<MeshRenderer>().enabled = state;
    }

    public void ResetHandles(){
        cube.localPosition = Vector3.zero;
        cube.localScale = initialCubeSize;
        PositionHandles();
    }

    public void PositionHandles(){
        initialCubeSize = cube.localScale;

        float axisCubeScaleFactor = 0f;
        for (int i = 0; i < handles.Length; i++)
        {
            if (handleDirections[i].x != 0)
            {
                axisCubeScaleFactor = cube.lossyScale.x/2;
            }
            else if (handleDirections[i].y != 0)
            {
                axisCubeScaleFactor = cube.lossyScale.y/2;
            }
            else if (handleDirections[i].z != 0)
            {
                axisCubeScaleFactor = cube.lossyScale.z/2;
            }

            handles[i].position = cube.position + handleDirections[i] * axisCubeScaleFactor;
        }
    }

    public void ScaleCube(Transform handle, Vector3 delta, Vector3 direction)
    {
        int index = System.Array.IndexOf(handles, handle);
        if (index == -1) return;

        // Convert delta to cube local space
        Vector3 localDelta = cube.InverseTransformDirection(delta);

        Vector3 localScaleChange = Vector3.zero;

        // Apply scale change along the correct local axis
        if (Mathf.Abs(direction.x) > 0)
        {
            localScaleChange.x = localDelta.x * Mathf.Sign(direction.x);
        }
        else if (Mathf.Abs(direction.y) > 0)
        {
            localScaleChange.y = localDelta.y * Mathf.Sign(direction.y);
        }
        else if (Mathf.Abs(direction.z) > 0)
        {
            localScaleChange.z = localDelta.z * Mathf.Sign(direction.z);
        }

        // Compute how to move the cube so the opposite side stays in place
        Vector3 localPositionOffset = Vector3.zero;
        if (direction.x != 0) localPositionOffset.x = localScaleChange.x / 2f * Mathf.Sign(direction.x);
        if (direction.y != 0) localPositionOffset.y = localScaleChange.y / 2f * Mathf.Sign(direction.y);
        if (direction.z != 0) localPositionOffset.z = localScaleChange.z / 2f * Mathf.Sign(direction.z);

        // Apply scale and position
        cube.localScale += localScaleChange;
        cube.position += cube.TransformDirection(localPositionOffset);

        var bounds = cube.GetComponent<Renderer>().bounds;
        // Reposition handles based on updated cube size and rotation
        for (int i = 0; i < handles.Length; i++)
        {
            Vector3 localOffset = Vector3.Scale(handleDirections[i], cube.localScale / 2f);
            Vector3 worldOffset = cube.TransformDirection(localOffset);

            handles[i].position = cube.position + worldOffset;

            
            ScalerOneGrabTranslateTransformer sogtt = handles[i].GetComponent<ScalerOneGrabTranslateTransformer>();
            ScalerOneGrabTranslateTransformer.OneGrabTranslateConstraints ogtc = new ScalerOneGrabTranslateTransformer.OneGrabTranslateConstraints()
            {
                ConstraintsAreRelative = true,
                MinX = new FloatConstraint(),
                MaxX = new FloatConstraint(),
                MinY = new FloatConstraint(),
                MaxY = new FloatConstraint(),
                MinZ = new FloatConstraint(),
                MaxZ = new FloatConstraint()
            };

            if (handleDirections[i] == Vector3.up || handleDirections[i] == Vector3.down)
            {
                ogtc.MinX.Constrain = true;
                ogtc.MaxX.Constrain = true;

                ogtc.MinY.Constrain = false;
                ogtc.MaxY.Constrain = false;

                ogtc.MinZ.Constrain = true;
                ogtc.MaxZ.Constrain = true;

                ogtc.MinZ.Value = handles[i].localPosition.z;
                ogtc.MaxZ.Value = handles[i].localPosition.z;

                ogtc.MinX.Value = handles[i].localPosition.x;
                ogtc.MaxX.Value = handles[i].localPosition.x;
            }
            else if (handleDirections[i] == Vector3.right || handleDirections[i] == Vector3.left)
            {
                ogtc.MinX.Constrain = false;
                ogtc.MaxX.Constrain = false;

                ogtc.MinY.Constrain = true;
                ogtc.MaxY.Constrain = true;

                ogtc.MinZ.Constrain = true;
                ogtc.MaxZ.Constrain = true;

                ogtc.MinZ.Value = handles[i].localPosition.z;
                ogtc.MaxZ.Value = handles[i].localPosition.z;

                ogtc.MinY.Value = handles[i].localPosition.y;
                ogtc.MaxY.Value = handles[i].localPosition.y;
            }
            else if (handleDirections[i] == Vector3.forward || handleDirections[i] == Vector3.back)
            {
                ogtc.MinX.Constrain = true;
                ogtc.MaxX.Constrain = true;

                ogtc.MinY.Constrain = true;
                ogtc.MaxY.Constrain = true;

                ogtc.MinZ.Constrain = false;
                ogtc.MaxZ.Constrain = false;

                ogtc.MinX.Value = handles[i].localPosition.x;
                ogtc.MaxX.Value = handles[i].localPosition.x;

                ogtc.MinY.Value = handles[i].localPosition.y;
                ogtc.MaxY.Value = handles[i].localPosition.y;
            }

            sogtt.Constraints = ogtc;
        }
    }
}
