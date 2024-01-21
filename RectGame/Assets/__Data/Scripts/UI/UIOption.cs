using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIOption : UIPopup
{
    [SerializeField] private Button closeButton;

    private void Awake()
    {
        ButtonActionAdd(ref closeButton, GameResume);
    }
}
