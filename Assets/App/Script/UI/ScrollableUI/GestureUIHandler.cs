using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GestureUIHandler
{
    public enum Direction { Horizontal, Vertical}
    private Direction _direction;
    private Camera _camera;
    private Vector2 lastMousePosition;
    private Vector2 scrollForce;
    private float _scrollThreshold;

    public System.Action OnMouseRelease;
    public System.Action OnMouseDragging;

    private ScrollViewCtrl _scrollViewCtrl;

    private Vector2 mouseDownPos, mouseUpPos, mouseWorldPos;
    private SwipeDataSet gestureData;

    public GestureUIHandler(ScrollViewCtrl scrollView, Direction p_direction, float scrollThreshold ,Camera camera) {
        this._scrollViewCtrl = scrollView;
        this._camera = camera;
        this._direction = p_direction;
        this._scrollThreshold = scrollThreshold;
        this.lastMousePosition = Vector2.zero;

        gestureData.timeThreshold = 0.2f;
        gestureData.forceThreshold = 1.5f;
    }

    public void OnUpdate() {

        if (Input.GetMouseButtonDown(0) && _scrollViewCtrl.inputType != ScrollViewCtrl.InputType.InnerUIActivity)
        {
            gestureData.MouseDownTime = Time.time;
            gestureData.MouseDownPos = GetUpdateMousePos();
        }


        if (Input.GetMouseButton(0) && _scrollViewCtrl.inputType != ScrollViewCtrl.InputType.InnerUIActivity)
        {
            CheckDragging();
        }

        if (Input.GetMouseButtonUp(0))
        {

            lastMousePosition = Vector2.zero;
            if (_scrollViewCtrl.inputType != ScrollViewCtrl.InputType.InnerUIActivity) {
                int gestureDir = DetectGesture();
                Release(gestureDir);
            }
        }
    }

    private Vector2 GetUpdateMousePos() {
        var mousePosition = _camera.ScreenToWorldPoint(Input.mousePosition);
        mouseWorldPos.Set(mousePosition.x, mousePosition.y);
        return mouseWorldPos;
    }

    private void CheckDragging() {
        GetUpdateMousePos();

        if (lastMousePosition == Vector2.zero)
        {
            lastMousePosition = mouseWorldPos;
            return;
        }

        var delta = mouseWorldPos - lastMousePosition;
        float absDeltaX = Mathf.Abs(delta.x), absDeltaY = Mathf.Abs(delta.y);
        var moveDist = Vector2.Distance(mouseWorldPos, lastMousePosition);

        if (moveDist > _scrollThreshold && absDeltaX > absDeltaY)
        {
            this._scrollViewCtrl.inputType = ScrollViewCtrl.InputType.OuterUIActivity;
        }


        if (this._scrollViewCtrl.inputType == ScrollViewCtrl.InputType.OuterUIActivity) {
            scrollForce.Set( (_direction == Direction.Horizontal) ? delta.x : 0,
                             (_direction == Direction.Vertical) ? delta.y : 0);

            this._scrollViewCtrl.Translate(scrollForce);
        }

        lastMousePosition = mouseWorldPos;
    }

    private void Dragging() {

    }

    private int DetectGesture() {

        gestureData.MouseUpTime = Time.time;
        gestureData.MouseUpPos = GetUpdateMousePos();

        return gestureData.SwipeDir(_direction);
    }

    private void Release(int indexDirection) {
        //lastMousePosition = Vector2.zero;
        var landIndex = this._scrollViewCtrl.mainUIIndex;

        if (indexDirection == 0)
        {
            landIndex = this._scrollViewCtrl.GetScrollIndexByPosition(this._scrollViewCtrl.scrollViewHolder.localPosition);
        }
        else {
            landIndex = this._scrollViewCtrl.CheckPageIndex(landIndex + indexDirection);
        }

        if (this._scrollViewCtrl.mainUIIndex != landIndex && this._scrollViewCtrl.inputType == ScrollViewCtrl.InputType.OuterUIActivity)
            this._scrollViewCtrl.ScrollToPage(landIndex);

        this._scrollViewCtrl.inputType = ScrollViewCtrl.InputType.Idle;
    }

    private struct SwipeDataSet {
        public float MouseDownTime, MouseUpTime;
        public Vector2 MouseDownPos, MouseUpPos;
        public float forceThreshold, timeThreshold;

        public int SwipeDir(Direction p_direction) {
            var timeDiff = MouseUpTime - MouseDownTime;
            float vectorDist;

            if (p_direction == Direction.Horizontal)
                vectorDist = MouseUpPos.x - MouseDownPos.x;
            else
                vectorDist = MouseUpPos.y - MouseDownPos.y;


            var vectorDiff = Mathf.Abs(vectorDist);

            if (timeDiff < this.timeThreshold && vectorDiff > this.forceThreshold) {
                return (vectorDist > 0) ? -1 : 1;
            }

            return 0;
        }
    }

}
