using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ModelPointerPositionCaster : MonoBehaviour
{
    [SerializeField] private TMP_InputField inputFieldX;
    [SerializeField] private TMP_InputField inputFieldY;
    [SerializeField] private TMP_InputField inputFieldZ;

    private void Update() {
        inputFieldX.text = this.transform.localPosition.x.ToString();
        inputFieldY.text = this.transform.localPosition.y.ToString();
        inputFieldZ.text = this.transform.localPosition.z.ToString();
    }
}
