using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class ClickEventHandler : MonoBehaviour, IPointerClickHandler
{
    [Header("Events")]
    [SerializeField] private UnityEvent onClick; // Event triggered when the GameObject is clicked

    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log("MASUK");
        // Trigger the onClick event when the GameObject is clicked
        onClick?.Invoke();
    }
}
