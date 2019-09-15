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

    private ScrollableView _scrollView;

    public GestureUIHandler(ScrollableView scrollView, Direction p_direction, float scrollThreshold ,Camera camera) {
        this._scrollView = scrollView;
        this._camera = camera;
        this._direction = p_direction;
        this._scrollThreshold = scrollThreshold;
        this.lastMousePosition = Vector2.zero;
    }

    public void OnUpdate() {
        if (_scrollView.inputType == ScrollableView.InputType.InnerUIActivity)
            return;

        if (Input.GetMouseButton(0))
        {
            CheckDragging();
        }

        if (Input.GetMouseButtonUp(0))
        {
            Release();
        }
    }

    private void CheckDragging() {
        var mousePosition = _camera.ScreenToWorldPoint(Input.mousePosition);
        var mousePosition2V = new Vector2(mousePosition.x, mousePosition.y); 

        if (lastMousePosition == Vector2.zero)
        {
            Debug.Log("lastMousePosition == Vector2.zero");
            lastMousePosition = mousePosition;
            return;
        }

        var delta = mousePosition2V - lastMousePosition;
        var moveDist = Vector2.Distance(mousePosition2V, lastMousePosition);

        if (moveDist > _scrollThreshold)
        {
            this._scrollView.inputType = ScrollableView.InputType.OuterUIActivity;
        }


        if (this._scrollView.inputType == ScrollableView.InputType.OuterUIActivity) {
            scrollForce.Set( (_direction == Direction.Horizontal) ? delta.x : 0,
                             (_direction == Direction.Vertical) ? delta.y : 0);

            this._scrollView.Translate(scrollForce);
        }

        lastMousePosition = mousePosition;
    }

    private void Dragging() {

    }

    private void Release() {
        Debug.Log("Release " );

        lastMousePosition = Vector2.zero;

        int landIndex = this._scrollView.GetScrollIndexByPosition(this._scrollView.transform.localPosition);

        if (this._scrollView.mainUIIndex != landIndex && this._scrollView.inputType == ScrollableView.InputType.OuterUIActivity)
            this._scrollView.ScrollToPage(landIndex);

        this._scrollView.inputType = ScrollableView.InputType.Idle;
    }

}
