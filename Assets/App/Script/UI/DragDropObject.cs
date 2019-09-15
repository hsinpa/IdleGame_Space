using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class DragDropObject : MonoBehaviour, IPointerUpHandler,  IPointerDownHandler
{
    /// <summary>
    /// bool true = drag, false = drop
    /// </summary>
    public System.Action<DragDropObject, bool> OnDragDropCallback;

    private TaskManagementMain taskManager;
    private DragDropHolder previousDragDropHolder;
    private int previousHolderIndex;


    public void Start()
    {
        this.taskManager = transform.GetComponentInParent<TaskManagementMain>();
    }

    public void SetUp(TaskManagementMain taskManager, System.Action<DragDropObject, bool> OnDragDropCallback) {
        this.taskManager = taskManager;
        this.OnDragDropCallback = OnDragDropCallback;
    }

    [SerializeField]
    private bool isDraggable;

    private void ExecuteCallback(bool isDrag) {

        if (OnDragDropCallback != null) {
            OnDragDropCallback(this, isDrag);
            return;
        }

        if (taskManager == null)
            return;

        taskManager.OnDragDropEvent(this, isDrag);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        ExecuteCallback(false);
        //blockImage.raycastTarget = true;

    }

    public void OnPointerDown(PointerEventData eventData)
    {
        previousDragDropHolder = this.GetComponentInParent<DragDropHolder>();
        previousHolderIndex = this.transform.GetSiblingIndex();

        //blockImage.raycastTarget = false;
        ExecuteCallback(true);
    }

    public void Reset() {

        if (previousDragDropHolder != null && previousHolderIndex >= 0) {
            previousDragDropHolder.Insert(this, previousHolderIndex);
        }

        previousDragDropHolder = null;
        previousHolderIndex = -1;
    }
}
