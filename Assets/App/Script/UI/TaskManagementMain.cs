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
    ScrollRectListener TaskPickScrollRect;

    [SerializeField]
    RectTransform DropArea;

    [SerializeField]
    TaskHolder taskHolder;

    [SerializeField]
    TaskProcessor taskProcessor;

    private RectTransform TaskPickScrollContent {
        get {
            return TaskPickScrollRect.scrollRect.content;
        }
    }

    private DragDropHolder[] dragDropHolders;
    private DragDropHolder currentDropHolder;

    private DragDropObject currentDragObject;
    private Camera _camera;

    private List<TaskDataSlot> taskDataSlots;
    private TaskCalculationHelper taskCalculationHelper;

    private TaskCalculationHelper.ParseResult taskParseResult;
    private ScrollableUI scrollableUI;

    private Vector3 mousePosition;
    private Vector2 lastScrollRectForce;

    // Start is called before the first frame update
    void Start()
    {
        if (taskProcessor == null)
            taskProcessor = GetComponent<TaskProcessor>();

        currentDropHolder = null;
        currentDragObject = null;
        _camera = Camera.main;
        taskDataSlots = new List<TaskDataSlot>();
        dragDropHolders = transform.GetComponentsInChildren<DragDropHolder>();
        taskCalculationHelper = new TaskCalculationHelper();
        scrollableUI = GetComponent<ScrollableUI>();

        TaskPickScrollRect.OnBeginDragEvent += OnUIBeginActivity;
        TaskPickScrollRect.OnEndDragEvent += OnUIEndActivitiy;

        taskProcessor.OnTaskDone += Init;

        AssignOnDropEvent(dragDropHolders);

        Init();
    }

    public void Init() {
        GeneratePickableTask(taskHolder);

        if (DropArea != null)
            UtilityMethod.ClearChildObject(DropArea);

        taskDataSlots.Clear();
        calculateResultText.text = "";
    }

    private void Update()
    {
        if (currentDragObject != null) {
            Vector3 worldMousePos = _camera.ScreenToWorldPoint(Input.mousePosition);
            mousePosition.Set(worldMousePos.x, worldMousePos.y, 90);
            //mousePosition = camera.ScreenToWorldPoint(mousePosition);

            currentDragObject.transform.position = mousePosition;
        }
    }

    private void UpdateCalculationResult() {
        taskParseResult = taskCalculationHelper.PredictTaskOutput(taskDataSlots);
        string outString = "";

        outString += "Effect\n" + taskParseResult.displayEffectText + "\n";
        outString += "Cost\n" + taskParseResult.displayCostText;

        calculateResultText.text = outString;

        taskProcessor.UpdateProcessData(taskParseResult);
    }

    private void GeneratePickableTask(TaskHolder p_taskHolder) {
        if (p_taskHolder != null && TaskPickScrollContent != null && 
            taskItemPrefab != null && p_taskHolder.stpObjectHolder.Count > 0) {
            TaskPickScrollContent.anchoredPosition =Vector2.zero;

            UtilityMethod.ClearChildObject(TaskPickScrollContent.transform);
            int taskObjectLength = p_taskHolder.stpObjectHolder.Count;

            VerticalLayoutGroup verticalLayout = TaskPickScrollContent.GetComponent<VerticalLayoutGroup>();
            RectTransform taskSlotRect = taskItemPrefab.GetComponent<RectTransform>();
            Vector2 taskSlotRectSize = taskSlotRect.sizeDelta;

            for (int i = 0; i < taskObjectLength; i++) {
                GameObject generateObject = UtilityMethod.CreateObjectToParent(TaskPickScrollContent.transform, taskItemPrefab);
                TaskDataSlot taskSlotObject = generateObject.GetComponent<TaskDataSlot>();

                taskSlotObject.SetUp(p_taskHolder.stpObjectHolder[i]);
            }

            TaskPickScrollContent.sizeDelta = new Vector2(TaskPickScrollContent.sizeDelta.x, ((taskObjectLength * taskSlotRectSize.y) + (verticalLayout.spacing * taskObjectLength) ));
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
        currentDropHolder = dropHolder;
    }

    private void OnHolderExistEvent(DragDropHolder dropHolder)
    {
        currentDropHolder = null;
    }

    public void OnDragDropEvent(DragDropObject ddObject, bool isDrag)
    {

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

        OnUIBeginActivity();
    }

    private void OnDropObject(DragDropObject ddObject)
    {
        //If current inside a drop holder
        if (currentDropHolder != null)
        {
            currentDropHolder.Insert(ddObject);

            TaskDataSlot task = GetTaskData(ddObject);
            if (task != null && currentDropHolder.mount)
            {
                taskDataSlots.Add(task);
                UpdateCalculationResult();
            }
        }
        else {
            ddObject.Reset();
        }

        OnUIEndActivitiy();
        currentDragObject = null;
    }

    private void OnUIBeginActivity()
    {
        if (scrollableUI != null)
            scrollableUI.NotifyUILock();
    }

    private void OnUIEndActivitiy()
    {
        if (scrollableUI != null)
            scrollableUI.NotifyUIRelease();
    }
    #endregion

    private TaskDataSlot GetTaskData(DragDropObject dragObject) {
        return dragObject.GetComponent<TaskDataSlot>();
    }
}