using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIBanner : UIDisplay
{
    [SerializeField] private Text scoreText;

    public void ScoreUpdate(float score)
    {
        scoreText.text = $"{score:0.0}";
    }
}
