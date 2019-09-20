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
    private bool activateScrolling;
    private bool isDragging;

    private Vector2 lastOffset;

    public void Awake() {
        activateScrolling = false;
        isDragging = false;
        _scrollRect = GetComponent<ScrollRect>();
        //_scrollRect.onValueChanged.AddListener(OnScrollValueChange);
    }


    private void OnScrollValueChange(Vector2 offset) {
        if (!isDragging) {
            lastOffset = Vector2.zero;
            return;
        }

        if (lastOffset == Vector2.zero || activateScrolling) {
            lastOffset = offset;
            return;
        }

        var offsetDiff = offset - lastOffset;
        var yDiff = Mathf.Abs(offsetDiff.y);
        var xDiff = Mathf.Abs(offsetDiff.x);

        if (yDiff > xDiff && !activateScrolling)
        {
            //OnBeginDrag();
            activateScrolling = true;
        }

        lastOffset = offset;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (OnEndDragEvent != null)
            OnEndDragEvent();

        activateScrolling = false;
        isDragging = false;
        lastOffset = Vector2.zero;
        Debug.Log("OnEndDrag");
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (OnBeginDragEvent != null)
            OnBeginDragEvent();
        
        Debug.Log("OnBeginDrag");
    }
}
