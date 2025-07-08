using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CrossSectionTransformList : MonoBehaviour
{
    [SerializeField] private Button[] csTransformBtn;

    private void Start() {
        SwitchBtnState(false);
    }
    
    public void SwitchBtnState(bool state){
        foreach (Button csBtn in csTransformBtn){
            csBtn.interactable = state;
        }
    }
}
