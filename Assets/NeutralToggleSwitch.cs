using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using System;

public class NeutralToggleSwitch : MonoBehaviour{

    private bool state = false;
    public RectTransform switchBtn;

    public bool switchOnStart;

    [SerializeField] private bool changeBgOnChange;
    [SerializeField] private RawImage bgImage;
    [SerializeField] private Color changeBGColor;
    private Color initBGColor;
        // Start is called before the first frame update
        private void Start() {
        if(changeBgOnChange){
            initBGColor = bgImage.color;
        }
        if(switchOnStart){
            OnSwitchButtonClicked();
        }
    }

    public void OnSwitchButtonClicked(){
        if(state){
            switchBtn.anchoredPosition = new Vector2(3, 0);
            if(changeBgOnChange){
                bgImage.color = initBGColor;
            }
            state = false;
            ToggleOffTrigger();
        }else{
            switchBtn.anchoredPosition = new Vector2(18, 0);
            if(changeBgOnChange){
                bgImage.color = changeBGColor;
            }
            state = true;
            ToggleOnTrigger();
        }
        
    }

    [Serializable] public class ToggleOnEvent : UnityEvent { }

	[SerializeField]
	private ToggleOnEvent toggleOnEvent = new ToggleOnEvent();
	public ToggleOnEvent onToggleOnEvent { get { return toggleOnEvent; } set { toggleOnEvent = value; } }

	public void ToggleOnTrigger(){
		onToggleOnEvent.Invoke();
	}

    [Serializable] public class ToggleOffEvent : UnityEvent { }

	[SerializeField]
	private ToggleOffEvent toggleOffEvent = new ToggleOffEvent();
	public ToggleOffEvent onToggleOffEvent { get { return toggleOffEvent; } set { toggleOffEvent = value; } }

	public void ToggleOffTrigger(){
		onToggleOffEvent.Invoke();
	}
}
