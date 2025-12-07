using UnityEngine;
using UnityEngine.EventSystems;

public class EventClick : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    // private MaterialApplier materialApplier;
    // private void Awake()
    // {
    //     materialApplier = GetComponent<MaterialApplier>();
    // }

    public void OnPointerDown(PointerEventData eventData)
    {
        Debug.Log("Pointer Down: " + eventData.pointerCurrentRaycast.worldPosition);
    }
        
    public void OnPointerUp(PointerEventData eventData)
    {
        Debug.Log("Pointer Up: " + eventData.pointerCurrentRaycast.worldPosition);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log("Pointer Click: " + eventData.pointerCurrentRaycast.worldPosition);
        //Debug.Log(eventData.pointerPressRaycast.worldPosition);
        // InputManager.Instance.
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        // Debug.Log("Pointer Enter");
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        // Debug.Log("Pointer Exit");
    }

}
