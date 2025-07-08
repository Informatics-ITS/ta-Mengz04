using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Oculus.Interaction;

public class ProbeLine : MonoBehaviour
{
    [SerializeField] private RectTransform labelTransform;
    private TMP_Text distLabel;
    private Vector3 tempPos;
    private float textHeight = 0.025f;
    private bool isDragging = false;
    [SerializeField] private Transform plane;
    [SerializeField] private Transform source;
    [SerializeField] private LineRenderer lineRenderer;
    [SerializeField] private GameObject ortogonalCGFT;
    [SerializeField] private GameObject paralelCGFT;

    private void Start() {
        distLabel = labelTransform.gameObject.GetComponent<TMP_Text>();
        tempPos = labelTransform.localPosition;

        lineRenderer.positionCount = 2;
        lineRenderer.SetPosition(0, source.localPosition);
        lineRenderer.SetPosition(1, plane.localPosition);

        labelTransform.gameObject.SetActive(false);
    }

    public void BeginDrag(){
        isDragging = true;
        labelTransform.gameObject.SetActive(true);
    }

    public void EndDrag(){
        ortogonalCGFT.SetActive(false);
        paralelCGFT.SetActive(false);

        StartCoroutine(EndCooldown());

    }

    void Update()
    {
        if(isDragging){
            lineRenderer.positionCount = 2;
            lineRenderer.SetPosition(0, source.localPosition);
            lineRenderer.SetPosition(1, plane.localPosition);

            tempPos.z = plane.localPosition.z - source.localPosition.z;
            //Vector3 localUp = transform.InverseTransformDirection(Vector3.up);
            //tempPos += localUp * textHeight; //RALAT
            labelTransform.localPosition = tempPos;

            distLabel.text = ((plane.localPosition.z - source.localPosition.z)*100f).ToString("n3") + " cm";
        }
    }

    private IEnumerator EndCooldown() {
        yield return new WaitForSeconds(1f);
        isDragging = false;
        labelTransform.gameObject.SetActive(false);

        ortogonalCGFT.SetActive(true);
        paralelCGFT.SetActive(true);

    }
}