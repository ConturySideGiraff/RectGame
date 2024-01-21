using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIBanner : UIDisplay
{
    [SerializeField] private Text levelText;
    [SerializeField] private Text scoreText;
    [SerializeField] private Button shopButton;

    private UIOption _uiOption;
    
    private void Awake()
    {
        _uiOption = UIManager.Instance.GetPopup<UIOption>();
        
        ButtonActionAdd(ref shopButton, OnShop);
    }

    public void LevelUpdate(int level)
    {
        levelText.text = $"{level}";
    }

    public void ScoreUpdate(float score)
    {
        scoreText.text = $"{score:0.0}";
    }

    private void OnShop()
    {
        GamePause();
        
        // TODO : 데이터 정보 넘겨주기 2
        
        _uiOption.On();
    }
}
