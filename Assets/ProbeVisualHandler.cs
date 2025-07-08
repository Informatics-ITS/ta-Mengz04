using Oculus.Voice.Bindings.Android;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProbeVisualHandler : MonoBehaviour
{
    [SerializeField] private LineRenderer lineRenderer;
    [SerializeField] private MeshRenderer ortMeshRenderer;
    [SerializeField] private MeshRenderer parMeshRenderer;

    [SerializeField] private Color normColor;
    [SerializeField] private Color grabColor;
    [SerializeField] private Color successColor;
    [SerializeField] private Material normMaterial;
    [SerializeField] private Material grabMaterial;
    [SerializeField] private Material successMaterial;

    public void ToggleNorm() {
        lineRenderer.SetColors(normColor, normColor);

        ortMeshRenderer.material = normMaterial;
        parMeshRenderer.material = normMaterial;
    }

    public void ToggleGrab() { 
        lineRenderer.SetColors(grabColor, grabColor);

        ortMeshRenderer.material = grabMaterial;
        parMeshRenderer.material = grabMaterial;

    }

    public void ToggleSuccess()
    {
        lineRenderer.SetColors(successColor, successColor);

        ortMeshRenderer.material = successMaterial;
        parMeshRenderer.material = successMaterial;

        StartCoroutine(NormCooldown());
    }

    private IEnumerator NormCooldown() {
        yield return new WaitForSeconds(1);
        ToggleNorm();
    }
}
