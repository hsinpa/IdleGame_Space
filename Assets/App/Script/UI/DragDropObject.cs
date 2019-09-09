using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class DragDropObject : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    /// <summary>
    /// bool true = drag, false = drop
    /// </summary>
    public System.Action<DragDropObject, bool> OnDragDropCallback;

    private TaskManagementMain taskManager;

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

    public void OnBeginDrag(PointerEventData eventData)
    {
            ExecuteCallback(true);
    }

    public void OnDrag(PointerEventData eventData)
    {
    }

    public void OnEndDrag(PointerEventData eventData)
    {
            ExecuteCallback(false);
    }

    private void ExecuteCallback(bool isDrag) {

        if (OnDragDropCallback != null) {
            OnDragDropCallback(this, isDrag);
            return;
        }

        if (taskManager == null)
            return;

        taskManager.OnDragDropEvent(this, isDrag);
    }
}
