using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ScrollRectListener : MonoBehaviour, IBeginDragHandler, IEndDragHandler
{
    private ScrollRect _scrollRect;
    public ScrollRect scrollRect {
        get {
            return _scrollRect;
        }
    }

    public System.Action OnBeginDragEvent;
    public System.Action OnEndDragEvent;

    public void Awake() {
        _scrollRect = GetComponent<ScrollRect>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (OnBeginDragEvent != null)
            OnBeginDragEvent();
        Debug.Log("OnBeginDrag");
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (OnEndDragEvent != null)
            OnEndDragEvent();

        Debug.Log("OnEndDrag");
    }
}
