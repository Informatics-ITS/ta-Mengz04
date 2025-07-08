#region Includes
using Oculus.Interaction;
using UnityEngine;
using UnityEngine.Events;
#endregion

namespace TS.DoubleSlider
{
    [RequireComponent(typeof(RectTransform))]
    public class XRDoubleSlider : MonoBehaviour
    {
        #region Variables

        [Header("References")]
        [SerializeField] private XRSingleSlider _sliderMin;
        [SerializeField] private XRSingleSlider _sliderMax;

        [Header("Configuration")]
        [SerializeField] private bool _setupOnStart;
        [SerializeField] private float _minValue;
        [SerializeField] private float _maxValue;
        [SerializeField] private float _minDistance;
        [SerializeField] private float _initialMinValue;
        [SerializeField] private float _initialMaxValue;

        [Header("Events")]
        public UnityEvent<float, float> OnValueChanged;

        private float prevMin;
        private float prevMax;

        public float MinValue
        {
            get { return _sliderMin.Value; }
        }
        public float MaxValue
        {
            get { return _sliderMax.Value; }
        }

        #endregion

        private void Awake()
        {
            if (_sliderMin == null || _sliderMax == null)
            {
#if UNITY_EDITOR || DEVELOPMENT_BUILD

                Debug.LogError("Missing slider min: " + _sliderMin + ", max: " + _sliderMax);
#endif
                return;
            }

        }
        private void Start()
        {
            if (!_setupOnStart) { return; }
            Setup(_minValue, _maxValue, _initialMinValue, _initialMaxValue);
        }

        public void Setup(float minValue, float maxValue, float initialMinValue, float initialMaxValue)
        {
            _minValue = minValue;
            prevMin = _minValue;
            _maxValue = maxValue;
            prevMax = _maxValue;
            _initialMinValue = initialMinValue;
            _initialMaxValue = initialMaxValue;

            _sliderMin.Setup(_initialMinValue, minValue, maxValue);
            _sliderMax.Setup(_initialMaxValue, minValue, maxValue);
        }


        private void Update() {
            if(Mathf.Approximately(prevMin, MinValue) && Mathf.Approximately(prevMax, MaxValue)) {return;}

            Debug.Log("min change");
            OnValueChanged.Invoke(MinValue, MaxValue);

            OneGrabTranslateTransformer ogtt = _sliderMin.GetComponent<OneGrabTranslateTransformer>();
            OneGrabTranslateTransformer.OneGrabTranslateConstraints ogtc = new OneGrabTranslateTransformer.OneGrabTranslateConstraints()
            {
                ConstraintsAreRelative = false,
                MinX = new FloatConstraint(),
                MaxX = new FloatConstraint(),
                MinY = new FloatConstraint(),
                MaxY = new FloatConstraint(),
                MinZ = new FloatConstraint(),
                MaxZ = new FloatConstraint()
            };

            ogtc.MinX.Constrain = true;
            ogtc.MaxX.Constrain = true;

            ogtc.MinY.Constrain = true;
            ogtc.MaxY.Constrain = true;

            ogtc.MinZ.Constrain = true;
            ogtc.MaxZ.Constrain = true;

            ogtc.MinZ.Value = ogtt.Constraints.MinZ.Value;
            ogtc.MaxZ.Value = ogtt.Constraints.MaxZ.Value;

            ogtc.MinY.Value = ogtt.Constraints.MinY.Value;
            ogtc.MaxY.Value = ogtt.Constraints.MaxY.Value;

            ogtc.MinX.Value = ogtt.Constraints.MinX.Value;
            ogtc.MaxX.Value = _sliderMax.transform.localPosition.x;

            ogtt.Constraints = ogtc;

            ogtt = _sliderMax.GetComponent<OneGrabTranslateTransformer>();
            ogtc = new OneGrabTranslateTransformer.OneGrabTranslateConstraints()
            {
                ConstraintsAreRelative = false,
                MinX = new FloatConstraint(),
                MaxX = new FloatConstraint(),
                MinY = new FloatConstraint(),
                MaxY = new FloatConstraint(),
                MinZ = new FloatConstraint(),
                MaxZ = new FloatConstraint()
            };

            ogtc.MinX.Constrain = true;
            ogtc.MaxX.Constrain = true;

            ogtc.MinY.Constrain = true;
            ogtc.MaxY.Constrain = true;

            ogtc.MinZ.Constrain = true;
            ogtc.MaxZ.Constrain = true;

            ogtc.MinZ.Value = ogtt.Constraints.MinZ.Value;
            ogtc.MaxZ.Value = ogtt.Constraints.MaxZ.Value;

            ogtc.MinY.Value = ogtt.Constraints.MinY.Value;
            ogtc.MaxY.Value = ogtt.Constraints.MaxY.Value;

            ogtc.MinX.Value = _sliderMin.transform.localPosition.x;
            ogtc.MaxX.Value = ogtt.Constraints.MaxX.Value;

            ogtt.Constraints = ogtc;

            prevMin = MinValue;
            prevMax = MaxValue;
        }
    }
}