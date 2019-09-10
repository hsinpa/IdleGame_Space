using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;

public class DragDropHolder : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public System.Action<DragDropHolder> OnHolderEnter;
    public System.Action<DragDropHolder> OnHolderExist;

    public bool mount = false;

    public void Insert(DragDropObject dropObject) {
        dropObject.transform.SetParent(transform);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (OnHolderEnter != null)
            OnHolderEnter(this);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (OnHolderExist != null)
            OnHolderExist(this);
    }
}
