using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class GameData
{
    public int xLen;
    public int yLen;
    public int correctCount;
    public int targetCount;
    public float score;
    public float duration;
    
    public GameData(int xLen, int yLen)
    {
        this.xLen = xLen;
        this.yLen = yLen;
        correctCount = 0;
        targetCount = CardCount / 2;
        score = InitScore;
        duration = 0;
    }
    
    public int CardCount => xLen * yLen;
    private float InitScore => CardCount * (CardCount - 1) * 0.5f * ScoreHandler.InitMultiple;
}