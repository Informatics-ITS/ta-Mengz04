using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Mathematics;
using System.Linq;
using TMPro;
using System.ComponentModel;
using System.Reflection;
using System;
using UnityEngine.Events;
using System.Xml.Serialization;

public class AlignProcedure : MonoBehaviour
{
    [Header("Guide modality")]
    [SerializeField] private GameObject rightController;
    // [SerializeField] private GameObject rightHand;
    [Header("Guide point")]
    [SerializeField] private GameObject handGuidePoint;
    [SerializeField] private GameObject controllerGuidePoint;
    [Header("Aligner reference")]
    [SerializeField] private GameObject mainContainer;
    [SerializeField] private GameObject modelObj;
    [SerializeField] private GameObject modelPointsContainer;
    [SerializeField] private GameObject spawnPrefab;
    [SerializeField] private GameObject pointerPrefab;
    [SerializeField] private GameObject pointerTargetPrefab;

    private int currentMarker = 0;
    [Header("Marker Materials")]
    [SerializeField] private Material leftMat;
    [SerializeField] private Material rightMat;
    [SerializeField] private Material chinMat;

    [Header("Marker Button")]
    [SerializeField] private List<TMP_Text> btnTextList;

    [SerializeField] private UnityEvent toggleGuideEvent;
    [SerializeField] private GameObject phantom;

    private bool isRegistered = false;
    private Vector3 modelCentroid;

    public int GetCurrentMarker()
    {
        return this.currentMarker;
    }

    public void SetCurrentMarker(int val)
    {
        isRegistered = false;
        this.currentMarker = val;

        if (targetPoints[currentMarker] != null) Destroy(targetPoints[currentMarker]);
        UpdateMarkerButton();
    }

    public void IncrementCurrentMarker()
    {
        if (currentMarker + 1 > 2)
        {
            currentMarker = -1;

            if (targetPoints.Count(item => item != null) >= 3) { 
                return;
            }
        }

        currentMarker++;

        if (targetPoints[currentMarker] != null) IncrementCurrentMarker();
        UpdateMarkerButton();
    }

    [Header("Target")]
    [SerializeField] private bool isPhantom = false;

    private bool allowedToAlign = true;
    private GameObject[] modelPoints = new GameObject[3];
    private GameObject[] targetPoints = new GameObject[3];

    public enum MarkerType
    {
        [Description("Left Eye")]
        LeftEye = 0,

        [Description("Right Eye")]
        RightEye = 1,

        [Description("Chin")]
        Chin = 2,

        [Description("None")]
        None = -1
    }

    public static class EnumHelper
    {
        public static string GetDescription(Enum value)
        {
            FieldInfo fi = value.GetType().GetField(value.ToString());

            if (fi != null &&
                Attribute.GetCustomAttribute(fi, typeof(DescriptionAttribute)) is DescriptionAttribute description)
            {
                return description.Description;
            }

            return value.ToString();
        }
    }

    public void UpdateMarkerButton() {
        for (int i = 0; i < 3; i++) {
            if (btnTextList[i] == null) continue;

            btnTextList[i].text = (targetPoints[i] == null) ? "Place" : "Replace";
        }
    }

    private void Start()
    {
        // eye left
        Vector3 leftVec = new Vector3(-0.2122326f, -0.3031707f, -0.04700778f);
        //Vector3 leftVec = new Vector3(-0.0333659984f, 1.01611996f, 0.700996041f);
        GameObject eyeLeft = Instantiate(spawnPrefab, leftVec, Quaternion.identity);
        //GameObject eyeLeft = Instantiate(spawnPrefab, new Vector3(-0.0333659984f,1.01611996f,0.700996041f), Quaternion.identity);
        modelPoints[0] = eyeLeft;
        eyeLeft.transform.SetParent(modelPointsContainer.transform, true);
        eyeLeft.transform.localPosition = leftVec;
        eyeLeft.name = "point M_eyeLeft";

        // eye right
        Vector3 rightVec = new Vector3(0.1945892f, -0.2915505f, -0.0462558f);
        //Vector3 rightVec = new Vector3(-0.0331720002f, 1.01911795f, 0.805956006f);

        GameObject eyeRight = Instantiate(spawnPrefab, rightVec, Quaternion.identity);
        //GameObject eyeRight = Instantiate(spawnPrefab, new Vector3(-0.0331720002f,1.01911795f,0.805956006f), Quaternion.identity);
        modelPoints[1] = eyeRight;
        eyeRight.transform.SetParent(modelPointsContainer.transform, true);
        eyeRight.transform.localPosition = rightVec;
        eyeRight.name = "point M_eyeRight";

        // chin
        Vector3 chinVec = new Vector3(-0.003856512f, -0.3206435f, -0.4312252f);
        //Vector3 chinVec = new Vector3(0.0659559965f, 1.02362597f, 0.752195001f);
        GameObject chin = Instantiate(spawnPrefab, chinVec, Quaternion.identity);
        //GameObject chin = Instantiate(spawnPrefab, new Vector3(0.0659559965f,1.02362597f,0.752195001f), Quaternion.identity);
        modelPoints[2] = chin;
        chin.transform.SetParent(modelPointsContainer.transform, true);
        chin.transform.localPosition = chinVec;
        chin.name = "point M_chin";

        if (!isPhantom)
        {
            // eye left
            eyeLeft.transform.localPosition = new Vector3(PlayerPrefs.GetFloat("eyeLeft_x"), PlayerPrefs.GetFloat("eyeLeft_y"), PlayerPrefs.GetFloat("eyeLeft_z"));

            // eye right
            eyeRight.transform.localPosition = new Vector3(PlayerPrefs.GetFloat("eyeRight_x"), PlayerPrefs.GetFloat("eyeRight_y"), PlayerPrefs.GetFloat("eyeRight_z"));

            // chin
            chin.transform.localPosition = new Vector3(PlayerPrefs.GetFloat("chin_x"), PlayerPrefs.GetFloat("chin_y"), PlayerPrefs.GetFloat("chin_z"));
        }

        UpdateMarkerButton();
    }

    private void SetParentToCentroid()
    {
        Vector3 centroid = Vector3.zero;

        foreach (GameObject child in modelPoints)
        {
            centroid += child.transform.position;
        }
        centroid /= 3;

        //modelCentroid = centroid - mainContainer.transform.position;
        //Debug.Log("Model centroid: " + modelCentroid);

        Vector3 offset = centroid - modelPointsContainer.transform.position;

        modelPointsContainer.transform.position = centroid;

        // Adjust children positions to keep them in the same world space position
        foreach (GameObject child in modelPoints)
        {
            child.transform.position -= offset;
        }
    }

    private Vector3 GetTargetCentroid()
    {
        Vector3[] targets = new Vector3[3];

        for (int i = 0; i < 3; i++)
        {
            targets[i] = targetPoints[i].transform.position;
        }

        return (targets[0] + targets[1] + targets[2]) / 3;
    }

    void Update()
    {
        if (rightController.active)
        {
            if(OVRInput.GetDown(OVRInput.Button.PrimaryHandTrigger, OVRInput.Controller.RTouch))
                CreateSpatialAnchor(controllerGuidePoint.transform);

            if (OVRInput.GetDown(OVRInput.Button.PrimaryIndexTrigger, OVRInput.Controller.RTouch))
                toggleGuideEvent.Invoke();

            if (OVRInput.GetDown(OVRInput.Button.One, OVRInput.Controller.RTouch))
                Register();

            if (OVRInput.GetDown(OVRInput.Button.Two, OVRInput.Controller.RTouch))
                phantom.SetActive(phantom.activeSelf);
        }

        if (isRegistered) {
            Vector3 translate = GetTargetCentroid() - modelPointsContainer.transform.position;
            mainContainer.transform.position += translate;
        }
    }

    public void PoseMarkHand()
    {

        if (!rightController.active)
        {
            CreateSpatialAnchor(handGuidePoint.transform);
        }
    }

    public void CreateSpatialAnchor(Transform guideSrc)
    {
        if (targetPoints.Count(item => item != null) >= 3 || currentMarker == -1) return;
        if (targetPoints[currentMarker] != null) Destroy(targetPoints[currentMarker]);

        GameObject prefab = Instantiate(spawnPrefab, guideSrc.position, Quaternion.identity);

        prefab.AddComponent<OVRSpatialAnchor>();
        MeshRenderer tempMeshRenderer = prefab.GetComponent<MeshRenderer>();
        tempMeshRenderer.material = (currentMarker == 0) ? leftMat : (currentMarker == 1) ? rightMat : chinMat;
        targetPoints[currentMarker] = prefab;

        IncrementCurrentMarker();
    }

    public void Register()
    {
        if (isRegistered || modelPoints.Count(item => item != null) != 3 || targetPoints.Count(item => item != null) != 3)
        {
            return;
        }

        isRegistered = true;
        SetParentToCentroid();
        ProceedAlignment();
    }

    public void DestroyTargetMarker(int index)
    {
        if (targetPoints[index] == null) return;

        Destroy(targetPoints[index]);
    }

    public void DestroyAllTarget()
    {
        foreach (GameObject item in targetPoints)
        {
            Destroy(item);
        }
    }

    private void ProceedAlignment()
    {
        modelObj.transform.position = Vector3.zero;
        modelObj.transform.rotation = Quaternion.identity;

        Vector3[] points = new Vector3[3];
        Vector3[] targets = new Vector3[3];
        for (int i = 0; i < 3; i++)
        {
            points[i] = modelPoints[i].transform.position;
            targets[i] = targetPoints[i].transform.position;
        }

        Vector3 centroidP = (points[0] + points[1] + points[2]) / 3;
        Vector3 centroidQ = (targets[0] + targets[1] + targets[2]) / 3;

        Vector3[] translatedPoints = new Vector3[3];
        Vector3[] translatedTargets = new Vector3[3];
        for (int i = 0; i < 3; i++)
        {
            translatedPoints[i] = points[i] - centroidP;
            // GameObject prefab = Instantiate(pointerPrefab, translatedPoints[i], Quaternion.identity);
            // prefab.name = "pointer model "+i;

            translatedTargets[i] = targets[i] - centroidQ;
            // prefab = Instantiate(pointerTargetPrefab, translatedTargets[i], Quaternion.identity);
            // prefab.name = "pointer target "+i;
        }

        float3x3 H = new float3x3(
            new float3(translatedPoints[0].x, translatedPoints[1].x, translatedPoints[2].x),
            new float3(translatedPoints[0].y, translatedPoints[1].y, translatedPoints[2].y),
            new float3(translatedPoints[0].z, translatedPoints[1].z, translatedPoints[2].z)
        );

        float3x3 G = new float3x3(
            new float3(translatedTargets[0].x, translatedTargets[1].x, translatedTargets[2].x),
            new float3(translatedTargets[0].y, translatedTargets[1].y, translatedTargets[2].y),
            new float3(translatedTargets[0].z, translatedTargets[1].z, translatedTargets[2].z)
        );

        // Compute the covariance matrix
        float3x3 covariance = math.mul(math.transpose(H), G);

        // Compute the SVD rotation
        quaternion qRotation = ModifiedSvd.svdRotation(covariance);
        Debug.Log(qRotation);

        mainContainer.transform.rotation *= math.conjugate(qRotation);

        Vector3 translate = centroidQ - modelPointsContainer.transform.position;
        mainContainer.transform.position += translate;

    }

    void RotateAroundPoint(Transform transform, Vector3 pivot, Quaternion rotation)
    {
        Vector3 shift = pivot - transform.position;

        transform.position += shift;
        transform.rotation *= rotation;
        transform.position -= shift;
    }

}
