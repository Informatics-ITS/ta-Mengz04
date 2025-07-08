using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScaleDebug : MonoBehaviour
{
    public Vector3 targetSize;
    public MeshRenderer renderer;
    // Start is called before the first frame update
    void Start()
    {      
        // Vector3 initSize = renderer.bounds.size;
        Debug.Log("Initial size: " + renderer.bounds.size);
        
        // targetSize = new Vector3(255f, 255f, 166f);
        // Vector3 scaleDownFactor = new Vector3(targetSize.x/initSize.x, targetSize.y/initSize.y, targetSize.z/initSize.z);
        // Debug.Log("Scale factor: " + scaleDownFactor);

        // transform.localScale *= scaleDownFactor.x;

        // Debug.Log("After scale size: " + renderer.bounds.size);
    }
}
