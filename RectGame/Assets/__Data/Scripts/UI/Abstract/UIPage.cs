using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
}
