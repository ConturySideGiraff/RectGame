using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIResult : UIPopup
{
    [SerializeField] private GameObject winObject;
    [SerializeField] private Button reTryButton;
    
    [SerializeField] private GameObject loseObject;

    private void Awake()
    {
        ButtonActionAdd(ref reTryButton, ReTry);
    }

    private void ReTry()
    {
        
    }
    
    public void Win(ref GameData gameData)
    {
        winObject.SetActive(true);
        loseObject.SetActive(false);        
    }

    public void Lose(ref GameData gameData)
    {
        winObject.SetActive(false);
        loseObject.SetActive(true);
    }
}
