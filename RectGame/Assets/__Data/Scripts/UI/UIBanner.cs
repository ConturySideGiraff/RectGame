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

    private UIShop _uiShop;
    private UISetting _uiSetting;
    
    private void Awake()
    {
        _uiShop = UIManager.Instance.GetPopup<UIShop>();
        _uiSetting = UIManager.Instance.GetPopup<UISetting>();
        
        ButtonActionAdd(ref settingButton, OnSetting);
        ButtonActionAdd(ref shopButton, OnShop);
    }

    public void ScoreUpdate(float score)
    {
        scoreText.text = $"{score:0.0}";
    }

    private void OnSetting()
    {
        GamePause();

        // TODO : 데이터 정보 넘겨주기 1

        _uiSetting.On();
    }

    private void OnShop()
    {
        GamePause();
        
        // TODO : 데이터 정보 넘겨주기 2
        
        _uiShop.On();
    }
}
