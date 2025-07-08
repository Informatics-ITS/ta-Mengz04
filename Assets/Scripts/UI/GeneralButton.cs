using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using TMPro;

public class GeneralButton : MonoBehaviour
{
    [SerializeField] private bool state = false;
    [Header("Configuration")]
    [SerializeField] private bool hasIcon;
    [SerializeField] private bool hasText;
    [SerializeField] private bool hasColor;

    [Header("Image Target")]
    [SerializeField] private Image targetImage;
    [Header("Text Target")]
    [SerializeField] private TextMeshProUGUI targetText;
    

    [Header("Active Attributes")]
    [SerializeField] private Sprite activeIcon;
    [SerializeField] private string activeText;
    [SerializeField] private Color activeColor;
    [SerializeField] private UnityEvent activeEvent;
    

    [Header("Inactive Attributes")]
    [SerializeField] private Sprite inactiveIcon;
    [SerializeField] private string inactiveText;
    [SerializeField] private Color inactiveColor;
    [SerializeField] private UnityEvent inactiveEvent;

    private void Start() {
        if(hasIcon) targetImage.sprite = (state)? activeIcon : inactiveIcon;
        if(hasColor) targetImage.color = (state)? activeColor : inactiveColor;
        if(hasText) targetText.text = (state)? activeText : inactiveText;
    }

    public void ToggleButton(){
        state = !state;
        if(hasIcon) targetImage.sprite = (state)? activeIcon : inactiveIcon;
        if(hasColor) targetImage.color = (state)? activeColor : inactiveColor;
        if(hasText) targetText.text = (state)? activeText : inactiveText;

        if(state){
            activeEvent.Invoke();
        }
        else{
            inactiveEvent.Invoke();
        }
    }
}
