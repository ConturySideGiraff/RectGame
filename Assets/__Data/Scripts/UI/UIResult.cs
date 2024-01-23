using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIResult : UIPopup
{
    [SerializeField] private Text gridText;
    [SerializeField] private Text scoreText;
    [SerializeField] private Text retryButtonText;
    [SerializeField] private Button reTryButton;

    private void Awake()
    {
        ButtonActionAdd(ref reTryButton, ReTry);
    }

    private void ReTry()
    {
        // Data Change
        
        GameManager.Instance.ChangeState(this, GameManager.GameState.GameInit); // State
    }

    private void Common(ref GameData gameData)
    {
        gridText.text = $"Grid : {gameData.xLen} x {gameData.yLen}";
        scoreText.text = $"Score : {gameData.score:0.0}";
    }

    public void Win(ref GameData gameData)
    {
        Common(ref gameData);
        retryButtonText.text = "Next";
    }

    public void Lose(ref GameData gameData)
    {
        Common(ref gameData);
        retryButtonText.text = "Retry";
    }
}
