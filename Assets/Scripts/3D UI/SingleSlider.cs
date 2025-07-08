using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SingleSlider : MonoBehaviour
{
    [Header("Target Value")]
    [SerializeField] private float targetScaleFactor;
    [SerializeField] private UnityEvent<float> onChange;


    [Header("Slider Handle")]
    [SerializeField] private Transform handle;
    [SerializeField] private float sliderScaleFactor;

    private void Update(){
        onChange.Invoke((float)handle.localPosition.x*targetScaleFactor/sliderScaleFactor);
    }
}
