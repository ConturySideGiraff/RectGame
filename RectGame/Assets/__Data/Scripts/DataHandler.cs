using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataHandler : MonoBehaviour
{
    [SerializeField] private GameData gameData;

    public float InitScore => gameData.InitScore;
    
    public GameData Init(GameData data)
    {
        gameData = data;
        return gameData;
    }

    public GameData GetData(Component sender)
    {
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
