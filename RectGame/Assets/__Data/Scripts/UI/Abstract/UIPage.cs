using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public abstract class UIPage : MonoBehaviour
{
    private RectTransform _rt;

    public RectTransform Rt
    {
        get
        {
            _rt ??= GetComponent<RectTransform>();
            return _rt;
        }
    }

    protected void ButtonActionAdd(ref Button button, UnityAction action)
    {
        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(action);
    }
}
