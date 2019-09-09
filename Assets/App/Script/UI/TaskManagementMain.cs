using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaskManagementMain : MonoBehaviour
{
    private DragDropHolder[] dragDropHolders;
    private DragDropHolder currentDropHolder;

    // Start is called before the first frame update
    void Start()
    {
        currentDropHolder = null;
        dragDropHolders = transform.GetComponentsInChildren<DragDropHolder>();
    }

    #region On UI Event Area
    private void AssignOnDropEvent(DragDropHolder[] p_dragDropHolders) {
        int l = p_dragDropHolders.Length;
        for (int i = 0; i < l; i++) {
            p_dragDropHolders[i].OnHolderEnter += OnHolderEnterEvent;
            p_dragDropHolders[i].OnHolderExist += OnHolderExistEvent;
        }
    }

    private void OnHolderEnterEvent(DragDropHolder dropHolder) {
        currentDropHolder = dropHolder;
    }

    private void OnHolderExistEvent(DragDropHolder dropHolder)
    {
        currentDropHolder = null;
    }

    public void OnDragDropEvent(DragDropObject ddObject, bool isDrag)
    {
        Debug.Log(ddObject.name + " state : " + isDrag);
    }
    #endregion
}