using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScaleManager : MonoBehaviour{
    [SerializeField] private Vector3 positiveDir;
    [SerializeField] private Vector3 negativeDir;
    private bool posOnHold = false;
    private bool negOnHold = false;
    [SerializeField] private Transform ctModel;

    public void ChangeNegOnHold(bool state){
        negOnHold = state;
    }

    public void ChangeposOnHold(bool state){
        posOnHold = state;
    }

    public void PosHoldScale(){
        posOnHold = true;
        PosHoldIter();
    }

    private void PosHoldIter(){
        if(!posOnHold) return;
        ctModel.localScale += positiveDir;
        StartCoroutine(PosHoldDelay(0.1f));
    }

    public void NegHoldScale(){
        negOnHold = true;
        NegHoldIter();
    }

    private void NegHoldIter(){
        if(!negOnHold) return;
        ctModel.localScale += negativeDir;
        StartCoroutine(NegHoldDelay(0.1f));
    }

    IEnumerator PosHoldDelay(float delay){
        yield return new WaitForSeconds(delay);
        PosHoldIter();
    }
    IEnumerator NegHoldDelay(float delay){
        yield return new WaitForSeconds(delay);
        NegHoldIter();
    }
}
