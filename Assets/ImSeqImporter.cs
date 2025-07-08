using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityVolumeRendering;
using System.IO;
using System.Linq;
using TMPro;

public class ImSeqImporter : MonoBehaviour{
    [SerializeField] private GameObject loadingUI;
    [SerializeField] private GameObject outerObject;
    [SerializeField] private VolumeRenderedObject volObj;
    [SerializeField] private GameObject meshContainer;
    [SerializeField] private bool isDeveloper;
    private string modelRelativePath = @"E:\DICOM\Load CT";
    [SerializeField] private VolumeRenderedObject volumeRenderedObject;
    [SerializeField] private GameObject[] sliceRenderingPlanes;
    [SerializeField] private Transform pointerContainer;

    [Header("Disposable box cutout")]
    [SerializeField] private bool isXR = false;
    [SerializeField] private GameObject boxCutout;
    [SerializeField] private GameObject cubeScaler;
    [SerializeField] private GameObject transaxialPlane;
    private string appPath;

    private void Start() {
        //appPath = Application.dataPath;
        appPath = isDeveloper? modelRelativePath : PlayerPrefs.GetString("SelectedPath");
        Debug.Log("Read From: "+ appPath);
        ModelImport(appPath);
    }

    public void CallImport(){
        ModelImport(appPath);
    }

    public async void ModelImport(string dir){
        loadingUI.SetActive(true);                                                                                 
        List<string> filePaths = Directory.GetFiles(dir).ToList();
        // Create importer
        //IImageSequenceImporter importer = ImporterFactory.CreateImageSequenceImporter(ImageSequenceFormat.ImageSequence);
        IImageSequenceImporter importer = ImporterFactory.CreateImageSequenceImporter(ImageSequenceFormat.DICOM);
        // Load list of DICOM series (normally just one series)
        IEnumerable<IImageSequenceSeries> seriesList = await importer.LoadSeriesAsync(filePaths); // takes a long of time

        VolumeDataset dataset = await importer.ImportSeriesAsync(seriesList.First());

        MeshRenderer meshRenderer = meshContainer.GetComponent<MeshRenderer>();

        CreateObjectInternal(dataset, meshContainer, meshRenderer, volObj, outerObject);

        meshRenderer.sharedMaterial.SetTexture("_DataTex", dataset.GetDataTexture()); // Perlu cek

        await volumeRenderedObject.UpdateMaterialPropertiesAsync(null);


        foreach(GameObject sliceRenderingPlane in sliceRenderingPlanes){
            SlicePlaneMat(sliceRenderingPlane, dataset);
        }

        // outerObject.transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);
        // meshContainer.transform.localScale = new Vector3(1f, 1f, 1f);

        float maxScale = Mathf.Max(meshContainer.transform.localScale.x, meshContainer.transform.localScale.y, meshContainer.transform.localScale.z);
        pointerContainer.localScale = new Vector3(maxScale, maxScale, maxScale);
        pointerContainer.localPosition = meshContainer.transform.localPosition;
        pointerContainer.localRotation = meshContainer.transform.localRotation;

        Vector3 tempLocalScale = meshContainer.transform.localRotation * meshContainer.transform.localScale/10f;
        transaxialPlane.transform.localScale = new Vector3(Mathf.Abs(tempLocalScale.x), Mathf.Abs(tempLocalScale.y), Mathf.Abs(tempLocalScale.z));

        if (boxCutout != null & cubeScaler != null){
            AdjustBoxCutout(meshContainer.transform.localScale, meshContainer.transform.localRotation);
        }

        loadingUI.SetActive(false);
        Debug.Log("FINISH");
    }


    private static void CreateObjectInternal(VolumeDataset dataset, GameObject meshContainer, MeshRenderer meshRenderer, VolumeRenderedObject volObj, GameObject outerObject, IProgressHandler progressHandler = null)
    {            
        meshContainer.transform.parent = outerObject.transform;
        meshContainer.transform.localScale = Vector3.one;
        meshContainer.transform.localPosition = Vector3.zero;
        meshContainer.transform.parent = outerObject.transform;
        outerObject.transform.localRotation = Quaternion.Euler(0.0f, 0.0f, 0.0f);

        meshRenderer.sharedMaterial = new Material(meshRenderer.sharedMaterial);
        volObj.meshRenderer = meshRenderer;
        volObj.dataset = dataset;

        const int noiseDimX = 512;
        const int noiseDimY = 512;
        Texture2D noiseTexture = NoiseTextureGenerator.GenerateNoiseTexture(noiseDimX, noiseDimY);

        UnityVolumeRendering.TransferFunction tf = TransferFunctionDatabase.CreateTransferFunction();
        Texture2D tfTexture = tf.GetTexture();
        volObj.transferFunction = tf;

        TransferFunction2D tf2D = TransferFunctionDatabase.CreateTransferFunction2D();
        volObj.transferFunction2D = tf2D;

        meshRenderer.sharedMaterial.SetTexture("_GradientTex", null);
        meshRenderer.sharedMaterial.SetTexture("_NoiseTex", noiseTexture);
        meshRenderer.sharedMaterial.SetTexture("_TFTex", tfTexture);

        meshRenderer.sharedMaterial.EnableKeyword("MODE_DVR");
        meshRenderer.sharedMaterial.DisableKeyword("MODE_MIP");
        meshRenderer.sharedMaterial.DisableKeyword("MODE_SURF");

        meshContainer.transform.localScale = new Vector3(Mathf.Abs(dataset.scale.x), Mathf.Abs(dataset.scale.y), Mathf.Abs(dataset.scale.z));
        Debug.Log("dataset scale: " + dataset.scale);
        meshContainer.transform.localRotation = dataset.rotation;
        Debug.Log("dataset rotation: " + dataset.rotation);

        // Normalise size (TODO: Add setting for diabling this?)
        float maxScale = Mathf.Max(dataset.scale.x, dataset.scale.y, dataset.scale.z);

        // volObj.transform.localScale = Vector3.one / maxScale;
        volObj.transform.localScale = Vector3.one; // using unnormalized dimension
    }

    private void SlicePlaneMat(GameObject sliceRenderingPlane, VolumeDataset dataset){
        Material sliceMat = sliceRenderingPlane.GetComponent<MeshRenderer>().sharedMaterial;
        sliceMat.SetTexture("_DataTex", dataset.GetDataTexture());
        sliceMat.SetTexture("_TFTex", volumeRenderedObject.transferFunction.GetTexture());
        sliceMat.SetMatrix("_parentInverseMat", volumeRenderedObject.transform.worldToLocalMatrix);
        sliceMat.SetMatrix("_planeMat", Matrix4x4.TRS(sliceRenderingPlane.transform.position, sliceRenderingPlane.transform.rotation, transform.lossyScale)); // TODO: allow changing scale

        SlicingPlane slicingPlaneComp = sliceRenderingPlane.GetComponent<SlicingPlane>();
        if(slicingPlaneComp == null){
            ExternalSlicingPlane externalSlicingPlane = sliceRenderingPlane.GetComponent<ExternalSlicingPlane>();
            externalSlicingPlane.targetObject = volumeRenderedObject;
        }else{
            slicingPlaneComp.targetObject = volumeRenderedObject;
        }
        
    }
    private void AdjustBoxCutout(Vector3 scale, Quaternion rotation){
        boxCutout.transform.localRotation = rotation;
        boxCutout.transform.localScale = scale;

        cubeScaler.transform.localScale = scale;
        cubeScaler.transform.localRotation = rotation;

        if (isXR){
            cubeScaler.GetComponent<XRCubeScaler>().PositionHandles();
        }
        else{
            cubeScaler.GetComponent<CubeScaler>().PositionHandles();
        }
        
    }
}
