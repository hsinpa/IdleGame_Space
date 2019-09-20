﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollableElement : MonoBehaviour
{

    private float _targetXPosition;
    public float targetXPosition
    {
        get
        {
            return _targetXPosition;
        }
    }

    RectTransform rectTransform;
    public System.Action OnUILock;
    public System.Action OnUIRelease;

    protected ScrollableViewManager.InputType _inputType;

    private bool isUILock = false;

    void Start()
    {
        isUILock = false;
        rectTransform = GetComponent<RectTransform>();
    }

    public void SetTargetXPosition(float positionX)
    {
        _targetXPosition = positionX;
    }

    public void NotifyUILock() {
        if (OnUILock != null && !isUILock && _inputType != ScrollableViewManager.InputType.OuterUIActivity) {
            isUILock = true;
            OnUILock();
        }
    }

    public void NotifyUIRelease() {
        if (OnUIRelease != null && isUILock && _inputType != ScrollableViewManager.InputType.OuterUIActivity)
        {
            isUILock = false;
            OnUIRelease();
        }
    }

    public virtual void OnIputTypeChange(ScrollableViewManager.InputType inputType) {
        _inputType = inputType;
    }

}