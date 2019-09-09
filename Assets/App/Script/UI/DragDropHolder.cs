using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;

public class DragDropHolder : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public System.Action<DragDropHolder> OnHolderEnter;
    public System.Action<DragDropHolder> OnHolderExist;

    public void OnPointerEnter(PointerEventData eventData)
    {
        Debug.Log("OnPointerEnter");
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        Debug.Log("OnPointerExit");
    }
}
