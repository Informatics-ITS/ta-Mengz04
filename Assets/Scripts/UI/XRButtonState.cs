using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class XRButtonState : MonoBehaviour
{
    [SerializeField] private Image targetImage;
    [SerializeField] private bool changeIcon = true;
    [SerializeField] private Sprite activeIcon;
    [SerializeField] private Sprite inactiveIcon;
    
    [SerializeField] private TMP_Text targetText;
    [SerializeField] private bool changeText = true;
    [SerializeField] private string activeText;
    [SerializeField] private string inactiveText;

    public void Toggle(bool state){
        if(changeIcon) targetImage.sprite = (state)? activeIcon : inactiveIcon;
        if(changeText) targetText.text = (state)? activeText : inactiveText; 
    }
}
