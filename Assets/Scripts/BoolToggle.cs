using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public class BoolToggle : MonoBehaviour
{
    [SerializeField] private bool state = false;
    [SerializeField] private UnityEvent<bool> toggleEvent;
    
    public void Toggle(){
        state = !state;
        toggleEvent.Invoke(state);
    }
}
