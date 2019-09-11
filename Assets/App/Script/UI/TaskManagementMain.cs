using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using IG.Database;
using Utility;

public class TaskManagementMain : MonoBehaviour
{
    [SerializeField]
    GameObject taskItemPrefab;

    [SerializeField]
    GameObject tempDragHolder;

    [SerializeField]
    Text calculateResultText;

    [SerializeField]
    GameObject TaskPickScrollView;

    [SerializeField]
    TaskHolder taskHolder;

    private DragDropHolder[] dragDropHolders;
    private DragDropHolder currentDropHolder;

    private DragDropObject currentDragObject;
    private Camera camera;

    private List<TaskDataSlot> taskDataSlots;
    private TaskCalculationHelper taskCalculationHelper;

    // Start is called before the first frame update
    void Start()
    {
        currentDropHolder = null;
        currentDragObject = null;
        camera = Camera.main;
        taskDataSlots = new List<TaskDataSlot>();
        dragDropHolders = transform.GetComponentsInChildren<DragDropHolder>();
        taskCalculationHelper = new TaskCalculationHelper();
        AssignOnDropEvent(dragDropHolders);
    }

    private void Update()
    {
        if (currentDragObject != null) {
            var mousePosition = Input.mousePosition;
                //mousePosition = camera.ScreenToWorldPoint(mousePosition);

            currentDragObject.transform.position = mousePosition;
        }
    }

    private void UpdateCalculationResult() {
        string outputString = taskCalculationHelper.PredictTaskOutput(taskDataSlots);

        Debug.Log("UpdateCalculationResult " + outputString);
        calculateResultText.text = outputString;
    }

    private void GeneratePickableTask(TaskHolder p_taskHolder) {
        if (p_taskHolder != null && TaskPickScrollView != null && 
            taskItemPrefab != null && p_taskHolder.stpObjectHolder.Count > 0) {

            UtilityMethod.ClearChildObject(TaskPickScrollView.transform);
            int taskObjectLength = p_taskHolder.stpObjectHolder.Count;

            for (int i = 0; i < taskObjectLength; i++) {
                GameObject generateObject = UtilityMethod.CreateObjectToParent(TaskPickScrollView.transform, taskItemPrefab);
                TaskDataSlot taskSlotObject = generateObject.GetComponent<TaskDataSlot>();

                taskSlotObject.SetUp(p_taskHolder.stpObjectHolder[i]);
            }
        }
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
        Debug.Log("OnHolderEnterEvent");
        currentDropHolder = dropHolder;
    }

    private void OnHolderExistEvent(DragDropHolder dropHolder)
    {
        Debug.Log("OnHolderExistEvent");
        currentDropHolder = null;
    }

    public void OnDragDropEvent(DragDropObject ddObject, bool isDrag)
    {
        Debug.Log(ddObject.name + " state : " + isDrag);

        if (isDrag)
            OnDragObject(ddObject);
        else
            OnDropObject(ddObject);
    }

    private void OnDragObject(DragDropObject ddObject)
    {
        currentDragObject = ddObject;

        if (tempDragHolder)
            currentDragObject.transform.SetParent(tempDragHolder.transform);

        TaskDataSlot task = GetTaskData(ddObject);
        if (task != null) {
            taskDataSlots.Remove(task);
            UpdateCalculationResult();
        }
    }

    private void OnDropObject(DragDropObject ddObject)
    {
        //If current inside a drop holder
        if (currentDropHolder != null) {
            Debug.Log("Insert");
            currentDropHolder.Insert(ddObject);

            TaskDataSlot task = GetTaskData(ddObject);
            if (task != null && currentDropHolder.mount) {
                taskDataSlots.Add(task);
                UpdateCalculationResult();
            }
        }




        currentDragObject = null;
    }
    #endregion

    private TaskDataSlot GetTaskData(DragDropObject dragObject) {
        return dragObject.GetComponent<TaskDataSlot>();
    }
}