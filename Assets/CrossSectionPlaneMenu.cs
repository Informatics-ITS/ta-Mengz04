using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CrossSectionPlaneMenu : MonoBehaviour{
    [SerializeField] private GameObject csPlane;
    [SerializeField] private Button addBtn;
    [SerializeField] private Button rmBtn;

    void Start(){
        SwitchCSPlane(false);
    }
    
    public void SwitchCSPlane(bool state){
        csPlane.SetActive(state);
        if(state){
            addBtn.interactable = false;
            rmBtn.interactable = true;
        }else{
            addBtn.interactable = true;
            rmBtn.interactable = false;
        }
    }
}
