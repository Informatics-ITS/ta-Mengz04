using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotationManager : MonoBehaviour
{
    [SerializeField] private Vector3 positiveDir;
    [SerializeField] private Vector3 negativeDir;
    [SerializeField] private bool localTranslation = true;
    private bool posOnHold = false;
    private bool negOnHold = false;
    [SerializeField] private Transform ctModel;

    public void ChangeLocalTr(bool state){
        localTranslation = state;
    }
    public void ChangePosOnHold(bool state){
        posOnHold = state;
    }
    public void ChangeNegOnHold(bool state){
        negOnHold = state;
    }
    public void PosTapRotate(){
        ctModel.Rotate(positiveDir, (localTranslation) ? Space.Self : Space.World);
    }

    public void PosHoldRotate(){
        posOnHold = true;
        PosHoldIter();
    }

    private void PosHoldIter(){
        if(!posOnHold) return;
        ctModel.Rotate(positiveDir, (localTranslation) ? Space.Self : Space.World);
        StartCoroutine(PosHoldDelay(0.1f));
    }

    public void NegTapRotate(){
        ctModel.Rotate(negativeDir, (localTranslation) ? Space.Self : Space.World);
    }

    public void NegHoldRotate(){
        negOnHold = true;
        NegHoldIter();
    }

    private void NegHoldIter(){
        if(!negOnHold) return;
        ctModel.Rotate(negativeDir, (localTranslation) ? Space.Self : Space.World);
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
