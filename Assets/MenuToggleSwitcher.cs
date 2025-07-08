using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuToggleSwitcher : MonoBehaviour{
    [SerializeField] private bool TurnOnStart;
    [SerializeField] private Toggle[] toggles;
    [SerializeField] private Button[] buttons;
    [SerializeField] private Slider[] sliders;
    
    private void Start() {
        ChangeTonggleState(TurnOnStart ? true : false);
    }

    public void ChangeTonggleState(bool state){
        foreach (Toggle toggle in toggles){
            toggle.isOn = state;
            toggle.interactable = state;
        }

        foreach(Button button in buttons){
            button.interactable = state;
        }

        foreach(Slider slider in sliders){
            slider.interactable = state;
        }
    }
}
