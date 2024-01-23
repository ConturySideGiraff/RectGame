using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIOption : UIPopup
{
    [SerializeField] private Button closeButton;
    [SerializeField] private Button retryButton;

    private void Awake()
    {
        ButtonActionAdd(ref closeButton, () =>
        {
            GameResume();
            Off();
        });
        ButtonActionAdd(ref retryButton, () =>
        {   
            GameManager.Instance.ChangeState(this, GameManager.GameState.GameInit);
        });
    }
}
