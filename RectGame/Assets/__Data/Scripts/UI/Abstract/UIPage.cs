using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public abstract class UIPage : MonoBehaviour
{
    private RectTransform _rt;
    
    private static GameManager.GameState _prevState;
    
    public RectTransform Rt
    {
        get
        {
            _rt ??= GetComponent<RectTransform>();
            return _rt;
        }
    }

    protected virtual void ButtonActionAdd(ref Button button, UnityAction action)
    {
        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(action);
    }

    protected virtual void GamePause()
    {
        _prevState = GameManager.Instance.State;
        
        GameManager.Instance.ChangeState(this, GameManager.GameState.GamePause);
    }


    protected virtual void GameResume()
    {
        GameManager.Instance.ChangeState(this, _prevState);
    }
}
