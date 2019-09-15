using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;

public class DragDropHolder : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public System.Action<DragDropHolder> OnHolderEnter;
    public System.Action<DragDropHolder> OnHolderExist;

    public int maximumItems = -1;
    public bool mount = false;



    public void Insert(DragDropObject dropObject) {
        Insert(dropObject, -1);
    }

    public void Insert(DragDropObject dropObject, int index)
    {
        //If Holder capacity is overwhelm
        int childCount = transform.childCount;
        if (maximumItems >= 0 && childCount >= maximumItems) {
            dropObject.Reset();
            return;
        }

        dropObject.transform.SetParent(transform);

        if (index >= 0)
            dropObject.transform.SetSiblingIndex(index);
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
