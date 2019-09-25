using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ObserverPattern;

public class ScrollViewCtrl : Observer
{
    
    RectTransform rectTransform;
    float width, height, yOffset;

    [SerializeField]
    private int _mainUIIndex = 0;

    [SerializeField]
    private Transform _scrollViewHolder;
    public Transform scrollViewHolder {
        get { return _scrollViewHolder; }
    }


    /// <summary>
    /// Current Main UI Index
    /// </summary>
    public int mainUIIndex
    {
        get { return _mainUIIndex; }
    }

    List<ScrollableElement> scrollableUIList = new List<ScrollableElement>();
    int scrollableUILength;

    /// <summary>
    /// Position of ScrollableView, instantiate here to avoid memory leak
    /// </summary>
    Vector3 recordPosContainer;

    private Camera _camera;

    public enum InputType
    {
        Idle, //Clicking, DO nothing
        InnerUIActivity, // Scroll Down/Top to view more information
        OuterUIActivity, //Change Content view,
        Scrolling
    }

    private InputType _inputType;
    public InputType inputType
    {
        get { return _inputType; }
        set
        {
            if (OnInputTypeChange != null)
                OnInputTypeChange(value);

            //Debug.Log(value.ToString());
            _inputType = value;
        }
    }
    public System.Action<InputType> OnInputTypeChange;

    private GestureUIHandler _gestureHandler;

    // Start is called before the first frame update
    void Start()
    {
        _camera = Camera.main;
        inputType = InputType.Idle;
        rectTransform = _scrollViewHolder.GetComponent<RectTransform>();

        this.width = rectTransform.rect.width;
        this.height = rectTransform.rect.height;
        this.yOffset = (_scrollViewHolder.parent.GetComponent<RectTransform>().rect.height - this.height) * 0.5f;

        this._gestureHandler = new GestureUIHandler(this, GestureUIHandler.Direction.Horizontal, 0.08f, _camera);

        TestSrollView();
        Debug.Log(rectTransform.rect);
    }

    void TestSrollView()
    {
        ScrollableElement[] scrollableUI = _scrollViewHolder.GetComponentsInChildren<ScrollableElement>();

        for (int i = 0; i < scrollableUI.Length; i++)
            Insert(scrollableUI[i]);
    }


    void Update()
    {
        UpdateScrollViewPos();

        _gestureHandler.OnUpdate();
    }

    public int CheckPageIndex(int index)
    {
        return Mathf.Clamp(index, 0, scrollableUILength - 1);
    }

    #region UI Scrolling
    public int GetScrollIndexByPosition(Vector3 pos)
    {
        float maxWidth = width * scrollableUILength;
        int estimateIndex = -Mathf.RoundToInt(pos.x / width);
        estimateIndex = CheckPageIndex(estimateIndex);

        return estimateIndex;
    }

    /// <summary>
    /// Trigger scroll event, animation to page {Index}
    /// </summary>
    /// <param name="i">Index of Page</param>
    public void ScrollToPage(int i)
    {
        _mainUIIndex = i;
    }

    /// <summary>
    /// Scroll UI View with transition effect
    /// </summary>
    private void UpdateScrollViewPos()
    {
        if (inputType == InputType.OuterUIActivity || _scrollViewHolder == null)
            return;

        float limitDist = 1f;
        float targetXPosition = width * (-_mainUIIndex);

        if (Mathf.Abs(_scrollViewHolder.localPosition.x - targetXPosition) < limitDist)
        {
            recordPosContainer.Set(targetXPosition, yOffset, 0);
            _scrollViewHolder.localPosition = recordPosContainer;
            //inputType = InputType.Idle;
            return;
        }

        float lerpXPosition = Mathf.Lerp(_scrollViewHolder.localPosition.x, targetXPosition, 0.12f);
        recordPosContainer.Set(lerpXPosition, yOffset, 0);
        _scrollViewHolder.localPosition = recordPosContainer;
    }

    /// <summary>
    /// Set the position of UI Element inside parent accroding to their width (No Transition)
    /// </summary>
    private void SetScrollableListPos()
    {

        for (int i = 0; i < scrollableUILength; i++)
        {
            float moveXPosition = (width * i);

            //scrollableUIList[i].SetTargetXPosition(moveXPosition);
            recordPosContainer.Set(moveXPosition, 0, 0);
            scrollableUIList[i].transform.localPosition = recordPosContainer;
        }
    }

    public void Translate(Vector2 force)
    {
        if (_scrollViewHolder != null)
            _scrollViewHolder.Translate(force);
    }
    #endregion

    #region Scroll UI List Management
    public void Insert(ScrollableElement scrollableUI, int index = -1)
    {
        scrollViewHolder.SetParent(scrollableUI.transform);
        scrollableUILength++;

        if (index >= 0)
        {
            scrollableUI.transform.SetSiblingIndex(index);
            scrollableUIList.Insert(index, scrollableUI);
        }
        else
        {
            scrollableUIList.Add(scrollableUI);
        }

        scrollableUI.OnUILock += OnInnerUILock;
        scrollableUI.OnUIRelease += OnInnerUIRelease;

        OnInputTypeChange += scrollableUI.OnIputTypeChange;

        SetScrollableListPos();
    }

    public void Remove(ScrollableElement scrollableUI)
    {
        scrollableUILength--;

        scrollableUI.OnUILock -= OnInnerUILock;
        scrollableUI.OnUIRelease -= OnInnerUIRelease;

        OnInputTypeChange = scrollableUI.OnIputTypeChange;

        SetScrollableListPos();
    }

    public void RemoveAt(int index)
    {
        scrollableUILength--;
        SetScrollableListPos();
    }

    #endregion

    #region Gesture
    private void OnInnerUILock()
    {
        inputType = InputType.InnerUIActivity;
    }

    private void OnInnerUIRelease()
    {
        inputType = InputType.Idle;
    }
    #endregion


}
