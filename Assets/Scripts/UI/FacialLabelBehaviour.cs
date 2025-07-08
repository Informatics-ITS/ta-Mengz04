using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FacialLabelBehaviour : MonoBehaviour
{
[SerializeField] private float minScale = 1.0f;
    [SerializeField] private float maxScale = 1.2f;
    [SerializeField] private float duration = 2.0f;

    private RectTransform rectTransform; 
    private bool scalingUp = true;
    private float elapsedTime = 0f; 

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        if (rectTransform == null)
        {
            Debug.LogError("RectTransformScaler requires a RectTransform component on the same GameObject.");
        }
    }

    private void Update()
    {
        if (rectTransform == null)
        {
            return;
        }

        elapsedTime += Time.deltaTime;

        float t = elapsedTime / duration;
        if (scalingUp)
        {
            rectTransform.localScale = Vector3.one * Mathf.Lerp(minScale, maxScale, t);
        }
        else
        {
            rectTransform.localScale = Vector3.one * Mathf.Lerp(maxScale, minScale, t);
        }

        if (elapsedTime >= duration)
        {
            scalingUp = !scalingUp; 
            elapsedTime = 0f; 
        }
    }
}
