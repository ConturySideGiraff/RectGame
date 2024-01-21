using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Serialization;

[Serializable]
public class Grid
{
    public int xLen;
    public int yLen;
    
    public Grid(int xLen, int yLen)
    {
        this.xLen = xLen;
        this.yLen = yLen;
    }
    
    public int CardCount => xLen * yLen;
}

[Serializable]
public class GridData : Grid
{
    public int xScale;
    public int yScale;
    public int xSpace;
    public int ySpace;

    public GridData(int xLen, int yLen, int xScale, int yScale, int xSpace, int ySpace) : base(xLen, yLen)
    {
        this.xScale = xScale;
        this.yScale = yScale;
        this.xSpace = xSpace;
        this.ySpace = ySpace;
    }
}

[Serializable]
public class GameData : GridData
{
    [Space]
    public int level = 1;
    public int correctCount;
    public int targetCount;
    public float score;

    public float InitScore => CardCount * (CardCount - 1) * 0.5f;

    public GameData(int xLen, int yLen, int xScale, int yScale, int xSpace, int ySpace) : base(xLen, yLen, xScale, yScale, xSpace, ySpace)
    {
        Reset();
    }

    public void Reset()
    {
        correctCount = 0;
        targetCount = CardCount / 2;
        score = InitScore;
    }
}

[Serializable]
public class GameResultData
{
    public int level;
    public float score;
    public int gridX;
    public int gridY;
}