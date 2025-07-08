using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;

public class ButtonStateToggle : MonoBehaviour
{
    [SerializeField] private ButtonCollapsibleToggle buttonCollapsibleToggle;
    [SerializeField] private GameObject gizmoTranslation;
    [SerializeField] private bool isActive;

    [Header("Icon")]
    [SerializeField] private Sprite activeIcon;
    [SerializeField] private Sprite inactiveIcon;
    [SerializeField] private Image stateImg;

    [Header("Color Locked")]
    [SerializeField] private Color activeColor;

    [Header("Color Unlocked")]
    [SerializeField] private Color inactiveColor;
    [SerializeField] private Image buttonColor;

    [Header("Text label")]
    [SerializeField] private string activeTxt;
    [SerializeField] private string inactiveTxt;
    [SerializeField] private TextMeshProUGUI text;

    [Header("Events")]
    [SerializeField] private UnityEvent<bool> inputEvents;
    [SerializeField] private UnityEvent GizmoEnable;
    [SerializeField] private UnityEvent GizmoDisable;

    private void Start() {
        ApplyChange();

    }
    public void ToggleState(){
        isActive = !isActive;
        ApplyChange();
    }

    private void ApplyChange(){
        stateImg.sprite = (isActive)? activeIcon : inactiveIcon;

        buttonColor.color = (isActive)? activeColor : inactiveColor;

        text.text = (isActive)? activeTxt : inactiveTxt;

        inputEvents.Invoke(!isActive);

        gizmoTranslation.SetActive((!buttonCollapsibleToggle.GetExtendedState())? isActive : false);
    }

    public void GizmoFocusEnabler(){
        if(isActive) return;
        GizmoEnable.Invoke();
    }
}
