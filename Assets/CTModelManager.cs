using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CTModelManager : MonoBehaviour
{
    private Transform ctTransform;
    private Vector3 initLoc;
    void Start(){
        ctTransform = GetComponent<Transform>();
        initLoc = ctTransform.position;
    }

    public void ResetPos(){
        ctTransform.position = initLoc;
    }

    public void ResetRot(){
        ctTransform.rotation = Quaternion.identity;
    }
}
