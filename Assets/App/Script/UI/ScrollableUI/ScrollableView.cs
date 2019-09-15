using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollableView : MonoBehaviour
{
    RectTransform rectTransform;
    float width, height, yOffset;

    public int mainUIIndex = 0;

    List<ScrollableUI> scrollableUIList = new List<ScrollableUI>();
    int scrollableUILength;
    Vector3 recordPosContainer;

    private Camera _camera;

    public enum InputType
    {
        Idle, //Clicking, DO nothing
        InnerUIActivity, // Scroll Down/Top to view more information
        OuterUIActivity, //Change Content view,
        Scrolling
    }

    public InputType inputType;

    private GestureUIHandler _gestureHandler;

    // Start is called before the first frame update
    void Start()
    {
        _camera = Camera.main;
        inputType = InputType.Idle;
        rectTransform = GetComponent<RectTransform>();

        this.width = rectTransform.rect.width;
        this.height = rectTransform.rect.height;
        this.yOffset = (Screen.height - this.height) * 0.5f;
        
        this._gestureHandler = new GestureUIHandler(this, GestureUIHandler.Direction.Horizontal, 0.05f, _camera);

        TestSrollView();
        Debug.Log(rectTransform.rect);
    }

    void TestSrollView() {
        ScrollableUI[] scrollableUI = transform.GetComponentsInChildren<ScrollableUI>();

        for (int i = 0; i < scrollableUI.Length; i++)
            Insert(scrollableUI[i]);
    }

    void Update() {
        UpdateScrollViewPos();

        _gestureHandler.OnUpdate();
    }


    public int GetScrollIndexByPosition(Vector3 pos)
    {
        float maxWidth = width * scrollableUILength;
        int estimateIndex = -Mathf.RoundToInt(pos.x / width);

        if (estimateIndex < 0)
            estimateIndex = 0;

        if (estimateIndex >= scrollableUILength)
            estimateIndex = scrollableUILength - 1;

        return estimateIndex;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="i">Index of Page</param>
    public void ScrollToPage(int i) {
        mainUIIndex = i;
    }

    private void UpdateScrollViewPos() {
        if (inputType == InputType.OuterUIActivity || inputType == InputType.InnerUIActivity)
            return;

        float limitDist = 1f;
        float targetXPosition = width * (-mainUIIndex);

        if (Mathf.Abs(this.transform.localPosition.x - targetXPosition) < limitDist) {
            recordPosContainer.Set(targetXPosition, this.yOffset, 0);
            this.transform.localPosition = recordPosContainer;
            inputType = InputType.Idle;
            return;
        }

        float lerpXPosition = Mathf.Lerp(this.transform.localPosition.x, targetXPosition, 0.12f);
        recordPosContainer.Set(lerpXPosition, this.yOffset, 0);
        this.transform.localPosition = recordPosContainer;

        inputType = InputType.Scrolling;
    }

    private void UpdateScrollableListPos() {
                                                                     
        for (int i = 0; i < scrollableUILength; i++)
        {
            float moveXPosition = (width * i);

            //scrollableUIList[i].SetTargetXPosition(moveXPosition);
            recordPosContainer.Set(moveXPosition, 0, 0);
            scrollableUIList[i].transform.localPosition = recordPosContainer;
        }
    }

    public void Insert(ScrollableUI scrollableUI, int index = -1)
    {
        scrollableUI.transform.SetParent(this.transform);
        scrollableUILength++;

        if (index >= 0)
        {
            scrollableUI.transform.SetSiblingIndex(index);
            scrollableUIList.Insert(index, scrollableUI);
        }
        else {
            scrollableUIList.Add(scrollableUI);
        }

        scrollableUI.OnUILock += OnInnerUILock;
        scrollableUI.OnUIRelease += OnInnerUIRelease;

        UpdateScrollableListPos();
    }

    public void Remove(ScrollableUI scrollableUI) {
        scrollableUILength--;

        scrollableUI.OnUILock -= OnInnerUILock;
        scrollableUI.OnUIRelease -= OnInnerUIRelease;

        UpdateScrollableListPos();
    }

    public void RemoveAt(int index) {
        scrollableUILength--;
        UpdateScrollableListPos();
    }

    public void Translate(Vector2 force) {
        this.transform.Translate(force);
    }

    #region Gesture
    private void OnInnerUILock() {
        Debug.Log("OnInnerUILock");
        inputType = InputType.InnerUIActivity;
    }

    private void OnInnerUIRelease() {
        Debug.Log("OnInnerUIRelease");
        inputType = InputType.Idle;
    }
    #endregion

}
