using UnityEngine;

public class CubeScaler : MonoBehaviour
{
    public Transform cube; // The cube to be scaled
    private Transform[] handles = new Transform[6]; // 6 handles
    private Vector3[] handleDirections = new Vector3[6];
    [SerializeField] private Material handleMat;
    [SerializeField] private Material cubeMat;
    [SerializeField] private Material transparentMat;
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
        handleDirections = new Vector3[] {
        Vector3.right, Vector3.left,   // X+ and X-
        Vector3.up, Vector3.down,      // Y+ and Y-
        Vector3.forward, Vector3.back  // Z+ and Z-
    };

        handles = new Transform[handleDirections.Length];

        for (int i = 0; i < handleDirections.Length; i++)
        {
            GameObject handle = new GameObject($"Handle_{i}");
            handle.transform.SetParent(transform);

            // Direction relative to cube's rotation
            Vector3 worldDir = cube.TransformDirection(handleDirections[i].normalized);


            float axisCubeScaleFactor = 0f;

            if (Mathf.Abs(cube.InverseTransformDirection(handleDirections[i]).x) > 0.5f)
                axisCubeScaleFactor = cube.localScale.x / 2;
            else if (Mathf.Abs(cube.InverseTransformDirection(handleDirections[i]).y) > 0.5f)
                axisCubeScaleFactor = cube.localScale.y / 2;
            else if (Mathf.Abs(cube.InverseTransformDirection(handleDirections[i]).z) > 0.5f)
                axisCubeScaleFactor = cube.localScale.z / 2;

            // Move handles in the cube's local space
            Vector3 localOffset = handleDirections[i].normalized * axisCubeScaleFactor;

            handle.transform.position = cube.position + localOffset;

            // Add collider
            BoxCollider collider = handle.AddComponent<BoxCollider>();
            collider.size = new Vector3(0.005f, 0.005f, 0.005f);

            // Mesh for visibility
            MeshFilter meshFilter = handle.AddComponent<MeshFilter>();
            meshFilter.mesh = CreateCubeMesh();

            MeshRenderer renderer = handle.AddComponent<MeshRenderer>();
            renderer.material = handleMat;

            // Handle drag setup
            HandleDrag dragScript = handle.AddComponent<HandleDrag>();
            dragScript.cubeScaler = this;
            dragScript.handleDirection = worldDir;
            dragScript.SetNormalMaterial(handleMat);
            dragScript.SetTransparentMaterial(transparentMat);

            handles[i] = handle.transform;
        }
    }

    public void SetVisibility(bool state){
        foreach(Transform handle in handles){
            if(handle.gameObject.TryGetComponent<HandleDrag>(out HandleDrag handleDrag)){
                handleDrag.ToggleVisibility(state);
            }
        }
        cube.gameObject.GetComponent<MeshRenderer>().material = (state)? cubeMat : transparentMat; 
    }

    public void ResetHandles(){
        cube.position = Vector3.zero;
        cube.localScale = initialCubeSize;
        PositionHandles();
    }

    public void PositionHandles(){

        for (int i = 0; i < handles.Length; i++)
        {
            float axisCubeScaleFactor = 0f;

            if (Mathf.Abs(cube.InverseTransformDirection(handleDirections[i]).x) > 0.5f)
                axisCubeScaleFactor = cube.localScale.x / 2;
            else if (Mathf.Abs(cube.InverseTransformDirection(handleDirections[i]).y) > 0.5f)
                axisCubeScaleFactor = cube.localScale.y / 2;
            else if (Mathf.Abs(cube.InverseTransformDirection(handleDirections[i]).z) > 0.5f)
                axisCubeScaleFactor = cube.localScale.z / 2;

            // Move handles in the cube's local space
            Vector3 localOffset = handleDirections[i].normalized * axisCubeScaleFactor;
            handles[i].position = cube.position + localOffset;
        }
    }

    public void ScaleCube(Transform handle, Vector3 delta, Vector3 direction)
    {
        int index = System.Array.IndexOf(handles, handle);
        if (index == -1) return;

        // Convert global direction into cube's local direction
        Vector3 localDirection = cube.InverseTransformDirection(direction).normalized;

        // Convert delta to local space too
        Vector3 localDelta = cube.InverseTransformDirection(delta);

        Vector3 scaleChange = Vector3.zero;
        Vector3 positionChange = Vector3.zero;

        // Determine scale change based on local direction
        if (Mathf.Abs(localDirection.x) > 0.5f)
        {
            scaleChange.x = localDelta.x * Mathf.Sign(localDirection.x);
            positionChange.x = scaleChange.x / 2f * Mathf.Sign(localDirection.x);
        }
        else if (Mathf.Abs(localDirection.y) > 0.5f)
        {
            scaleChange.y = localDelta.y * Mathf.Sign(localDirection.y);
            positionChange.y = scaleChange.y / 2f * Mathf.Sign(localDirection.y);
        }
        else if (Mathf.Abs(localDirection.z) > 0.5f)
        {
            scaleChange.z = localDelta.z * Mathf.Sign(localDirection.z);
            positionChange.z = scaleChange.z / 2f * Mathf.Sign(localDirection.z);
        }

        // Update scale
        cube.localScale += scaleChange;

        // Update position (in world space)
        cube.position += cube.TransformDirection(positionChange);

        // Update handle positions
        for (int i = 0; i < handles.Length; i++)
        {
            float axisCubeScaleFactor = 0f;

            if (Mathf.Abs(cube.InverseTransformDirection(handleDirections[i]).x) > 0.5f)
                axisCubeScaleFactor = cube.localScale.x / 2;
            else if (Mathf.Abs(cube.InverseTransformDirection(handleDirections[i]).y) > 0.5f)
                axisCubeScaleFactor = cube.localScale.y / 2;
            else if (Mathf.Abs(cube.InverseTransformDirection(handleDirections[i]).z) > 0.5f)
                axisCubeScaleFactor = cube.localScale.z / 2;

            // Move handles in the cube's local space
            Vector3 localOffset = handleDirections[i].normalized * axisCubeScaleFactor;
            handles[i].position = cube.position + localOffset;
        }
    }


    private Mesh CreateCubeMesh()
    {
        Mesh mesh = new Mesh();

        Vector3[] vertices = {
        new Vector3(-0.005f, -0.005f, -0.005f), // 0
        new Vector3( 0.005f, -0.005f, -0.005f), // 1
        new Vector3( 0.005f,  0.005f, -0.005f), // 2
        new Vector3(-0.005f,  0.005f, -0.005f), // 3
        new Vector3(-0.005f, -0.005f,  0.005f), // 4
        new Vector3( 0.005f, -0.005f,  0.005f), // 5
        new Vector3( 0.005f,  0.005f,  0.005f), // 6
        new Vector3(-0.005f,  0.005f,  0.005f)  // 7
    };

    int[] triangles = {
        0, 2, 1, 0, 3, 2,
        1, 2, 6, 1, 6, 5,
        5, 6, 7, 5, 7, 4,
        4, 7, 3, 4, 3, 0,
        3, 7, 6, 3, 6, 2,
        0, 1, 5, 0, 5, 4
    };

        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.RecalculateNormals();

        return mesh;
    }
}
