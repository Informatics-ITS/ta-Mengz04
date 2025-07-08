using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerPrefsHandler : MonoBehaviour
{
    [SerializeField] private Transform pointerContainer;
    [SerializeField] private Transform eyeLeft;
    [SerializeField] private Transform eyeRight;
    [SerializeField] private Transform chin;
    [SerializeField] private UnityEvent followUpEvent;
    public void SetFloatPlayerPrefs(string key, float value){
        PlayerPrefs.SetFloat(key, value);
    }

    public void FinalizePoints(){
        // left eye
        SetFloatPlayerPrefs("eyeLeft_x", eyeLeft.position.x - pointerContainer.position.x);
        SetFloatPlayerPrefs("eyeLeft_y", eyeLeft.position.y - pointerContainer.position.y);
        SetFloatPlayerPrefs("eyeLeft_z", eyeLeft.position.z - pointerContainer.position.z);

        // right eye
        SetFloatPlayerPrefs("eyeRight_x", eyeRight.position.x - pointerContainer.position.x);
        SetFloatPlayerPrefs("eyeRight_y", eyeRight.position.y - pointerContainer.position.y);
        SetFloatPlayerPrefs("eyeRight_z", eyeRight.position.z - pointerContainer.position.z);

        // right eye
        SetFloatPlayerPrefs("chin_x", chin.position.x - pointerContainer.position.x);
        SetFloatPlayerPrefs("chin_y", chin.position.y - pointerContainer.position.y);
        SetFloatPlayerPrefs("chin_z", chin.position.z - pointerContainer.position.z);

        followUpEvent.Invoke();
    }
}
