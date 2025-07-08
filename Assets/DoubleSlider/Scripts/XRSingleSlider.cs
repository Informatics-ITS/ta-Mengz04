#region Includes
using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.PlayerLoop;
using UnityEngine.UI;
using TMPro;
#endregion

namespace TS.DoubleSlider
{
    public class XRSingleSlider : MonoBehaviour
    {
        #region Variables
        [Header("source config")]
        [SerializeField] private float srcMin;
        [SerializeField] private float srcMax;
        [SerializeField] private Transform handle;
        [Header("Target slider config")]
        [SerializeField] private float minValue;
        [SerializeField] private float maxValue;
        private float sliderVal;

        [Header("References")]
        [SerializeField] private TMP_Text _label;

        public float Value
        {
            get {
                sliderVal = ((handle.localPosition.x-srcMin)*(maxValue-minValue)/(srcMax-srcMin))+minValue;
                return sliderVal;
                }
            set
            {
                sliderVal = value;
                Vector3 tempPos = handle.position;
                tempPos.x = ((sliderVal-minValue)*(srcMax-srcMin)/(maxValue-minValue))+srcMin;
                handle.localPosition = tempPos;
            }
        }
        #endregion


        public void Setup(float value, float minValue, float maxValue)
        {
            this.minValue = minValue;
            this.maxValue = maxValue;

            sliderVal = value;
            Vector3 tempPos = handle.position;
            tempPos.x = ((sliderVal-minValue)*(srcMax-srcMin)/(maxValue-minValue))+srcMin;
            handle.position = tempPos;
        }


        private void Update() {
            if (_label == null) return;
            _label.text = (Value*255f).ToString("n0");
        }
    }
}