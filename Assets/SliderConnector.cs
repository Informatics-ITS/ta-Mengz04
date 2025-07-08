using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SliderConnector : MonoBehaviour{
    [SerializeField] private Slider slider;
    [SerializeField] private Transform target;

    [SerializeField] private bool translateX;
    [SerializeField] private bool translateY;
    [SerializeField] private bool translateZ;
    void Start()
    {
        
    }

    public void AdjustChange(float val){
        if(translateX){
            target.localPosition = new Vector3(val, 0, 0);
        }else if(translateY){
            target.localPosition = new Vector3(0, val, 0);
        }else{
            target.localPosition = new Vector3(0, 0, val);
        }
    }

    // Update is called once per frame
    void Update(){
        
    }
}
