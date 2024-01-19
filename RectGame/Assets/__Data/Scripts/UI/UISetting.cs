using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UISetting : UIPopup
{
    [SerializeField] private Button closeButton;

    private void Awake()
    {
        ButtonActionAdd(ref closeButton, GameResume);
    }
}
