using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIBanner : UIDisplay
{
    [SerializeField] private Text scoreText;
    [SerializeField] private Button settingButton;
    [SerializeField] private Button shopButton;

    private void Awake()
    {
        ButtonActionAdd(ref settingButton, OnSetting);
        ButtonActionAdd(ref shopButton, OnShop);
    }

    public void ScoreUpdate(float score)
    {
        scoreText.text = $"{score:0.0}";
    }

    private void OnSetting()
    {
        
    }

    private void OnShop()
    {
        
    }
}
