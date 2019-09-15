using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollableUI : MonoBehaviour
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
        if (OnUILock != null && !isUILock) {
            isUILock = true;
            OnUILock();
        }
    }

    public void NotifyUIRelease() {
        if (OnUIRelease != null && isUILock)
        {
            isUILock = false;
            OnUIRelease();
        }
    }


}
