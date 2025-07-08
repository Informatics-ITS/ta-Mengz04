using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TranslationManager : MonoBehaviour
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
    public void PosTapTranslate(){
        ctModel.Translate(positiveDir, (localTranslation) ? Space.Self : Space.World);
    }

    public void PosHoldTranslate(){
        posOnHold = true;
        PosHoldIter();
    }

    private void PosHoldIter(){
        if(!posOnHold) return;
        ctModel.Translate(positiveDir, (localTranslation) ? Space.Self : Space.World);
        StartCoroutine(PosHoldDelay(0.1f));
    }

    public void NegTapTranslate(){
        ctModel.Translate(negativeDir, (localTranslation) ? Space.Self : Space.World);
    }

    public void NegHoldTranslate(){
        negOnHold = true;
        NegHoldIter();
    }

    private void NegHoldIter(){
        if(!negOnHold) return;
        ctModel.Translate(negativeDir, (localTranslation) ? Space.Self : Space.World);
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
