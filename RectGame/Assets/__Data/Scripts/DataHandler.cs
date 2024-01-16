using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataHandler : MonoBehaviour
{
    [SerializeField] private GameData gameData;

    public GameData Init(int xLen, int yLen)
    {
        gameData.xLen = xLen;
        gameData.yLen = yLen;

        return gameData;
    }

    public GameData Init(GameData data)
    {
        gameData = data;
        return gameData;
    }

    public void Reset()
    {
        gameData = new GameData(gameData.xLen, gameData.yLen);
    }

    public bool OnCorrect()
    {
        gameData.correctCount += 1;

        return gameData.correctCount >= gameData.targetCount;
    }

    public void OnScoreUpdate(float score)
    {
        gameData.score = score;
    }
}
