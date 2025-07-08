using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonCollapsibleToggle : MonoBehaviour
{
    [SerializeField] private bool extended;
    [SerializeField] private Image outlinePanel;
    [SerializeField] private Color extendedColor;
    [SerializeField] private Color collapsedColor;
    [SerializeField] private GameObject collapsibleObj;
    [SerializeField] private RectTransform collapsibleIcon;

    [SerializeField] private RectTransform parentRect;

    public bool GetExtendedState(){
        return this.extended;
    }

    private void Start() {
        collapsibleIcon.rotation = Quaternion.Euler(collapsibleIcon.rotation.eulerAngles.x, collapsibleIcon.rotation.eulerAngles.y, (extended)? -90: 0);
        collapsibleObj.SetActive(extended);
        outlinePanel.color = (extended)? extendedColor : collapsedColor;
    }
    public void ToggleExtend(){
        if(extended) return;
        extended = !extended;
        collapsibleIcon.rotation = Quaternion.Euler(collapsibleIcon.rotation.eulerAngles.x, collapsibleIcon.rotation.eulerAngles.y, (extended)? -90: 0);
        collapsibleObj.SetActive(extended);
        outlinePanel.color = (extended)? extendedColor : collapsedColor;
        UpdateParentLayout();
    }

    public void ToggleCollapseSecondary(){
        if(!extended) return;
        extended = !extended;
        collapsibleIcon.rotation = Quaternion.Euler(collapsibleIcon.rotation.eulerAngles.x, collapsibleIcon.rotation.eulerAngles.y, (extended)? -90: 0);
        collapsibleObj.SetActive(extended);
        outlinePanel.color = (extended)? extendedColor : collapsedColor;
        UpdateParentLayout();
    }

    private void UpdateParentLayout()
    {
        if (parentRect != null)
        {
            LayoutRebuilder.ForceRebuildLayoutImmediate(parentRect);
        }
    }
}
