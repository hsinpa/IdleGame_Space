using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PM.View;
using PM.Model;

public class GanttChartDragging : MonoBehaviour
{
    [SerializeField]
    private Transform GanttLeftPanel;

    [SerializeField]
    private Transform GanttRightPanel;

    [SerializeField]
    private GanttChartView ganttChartView;


    private GroupTitle[] LeftGroupObject;
    private GroupContent[] RightGroupObject;

    private Vector3 moveDelta;
    private Vector3 lastMousePosition;
    private Vector3 initMousePosition;

    public float dragDiff = 0.8f;
    public float dragPower = 2f;

    private enum State {
        Drag,
        Idle
    }

    private enum DragDirection
    {
        Horizontal,
        Vertical
    }

    private State currentState;
    private DragDirection dragDirection;
    private PMUtility pm_utility;

    private Camera camera;

    void Start() {
        camera = Camera.main;
        currentState = State.Idle;

        RightGroupObject = GanttRightPanel.GetComponentsInChildren<GroupContent>();
        LeftGroupObject = GanttLeftPanel.GetComponentsInChildren<GroupTitle>();

        pm_utility = new PMUtility();
        ganttChartView.SetUp(pm_utility);
    }

    void Update() {

        Vector3 mouseWorldPos = camera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, camera.nearClipPlane));


        if (Input.GetMouseButtonDown(0)) {
            initMousePosition = mouseWorldPos;
            lastMousePosition = mouseWorldPos;

            currentState = State.Idle;
        }

        if (Input.GetMouseButton(0) && initMousePosition != Vector3.negativeInfinity) {

            
            if (Vector3.Distance(mouseWorldPos, initMousePosition) > dragDiff && currentState == State.Idle)
            {
                Vector3 diff = mouseWorldPos - initMousePosition;
                float vDiff = Mathf.Abs(diff.y);
                float hDiff = Mathf.Abs(diff.x);
                dragDirection = (vDiff > hDiff) ? DragDirection.Vertical : DragDirection.Horizontal;

                currentState = State.Drag;
            }
            else if (currentState == State.Drag) {
                //Debug.Log(mouseWorldPos +", "+ lastMousePosition);

                moveDelta = (mouseWorldPos - lastMousePosition);
                DragObject(moveDelta * dragPower, dragDirection);
            }

            lastMousePosition = mouseWorldPos;
        }

        if (Input.GetMouseButtonUp(0)) {
            initMousePosition = Vector3.negativeInfinity;
            currentState = State.Idle;
        }
    }

    void DragObject(Vector2 direction, DragDirection dragDirection) {
        if (dragDirection == DragDirection.Horizontal)
            direction.y = 0;

        if (dragDirection == DragDirection.Vertical)
            direction.x = 0;

        //Left Group only work with vertical dragging
        if (dragDirection == DragDirection.Vertical)
        {
            GanttLeftPanel.position = new Vector3(GanttLeftPanel.position.x + direction.x, GanttLeftPanel.position.y + direction.y, GanttLeftPanel.position.z);
            GanttRightPanel.position = new Vector3(GanttRightPanel.position.x + direction.x, GanttRightPanel.position.y + direction.y, GanttRightPanel.position.z);
        }
        else {
            GanttRightPanel.position = new Vector3(GanttRightPanel.position.x + direction.x, GanttRightPanel.position.y + direction.y, GanttRightPanel.position.z);
        }
    }

    void MoveGroupObject<T>(T[] objectArray, Vector3 force) where T : MonoBehaviour {
        if (objectArray == null) return;

        foreach (T gameObject in objectArray) {
            gameObject.transform.Translate(force);
        }
    }

}
