using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class DataManager : SingletonManager<DataManager>
{
    [FormerlySerializedAs("isLocal")]
    [Header("[ Local ]")] 
    [SerializeField] private bool isNetwork;
    public bool IsNetwork => isNetwork;
    
    [Header("[ Data ]")]
    [SerializeField, ReadOnly] private GameData gameData;

    public float InitScore => gameData.InitScore;
    public int Level => gameData.level;
    
    [Header("[ Card Sprite List]")]
    [SerializeField] private List<Sprite> cardBackSpriteList = new List<Sprite>();
    [SerializeField] private List<Sprite> cardFrontSpriteList = new List<Sprite>();
   
    public List<Sprite> CardBackSpriteList => cardBackSpriteList;
    public List<Sprite> CardFrontSpriteList => cardFrontSpriteList;

    //
    
    private UIBanner _uiBanner;

    //
    
    private void Awake()
    {
        _uiBanner = UIManager.Instance.GetDisplay<UIBanner>();
    }

    #region Game
    public GameData Init(GameData data)
    {
        gameData = data;
        return gameData;
    }

    public GameData GetData(Component sender)
    {
        return gameData;
    }

    public void Reset() => gameData.Reset();

    public bool OnCorrect()
    {
        gameData.correctCount += 1;

        return gameData.correctCount >= gameData.targetCount;
    }

    public void OnScoreUpdate(float score)
    {
        gameData.score = score;
    }
    #endregion

    #region Handler
    public int LevelLoad()
    {
        int level = isNetwork ? LevelLoadNetwork() : LevelLoadLocal();
        
        _uiBanner.LevelUpdate(level);
        
        return level;
    }
    #endregion
    
    #region Local

    private int LevelLoadLocal()
    {
        gameData.level = 3;
        
        return gameData.level;
    }
    #endregion

    #region Network

    private int LevelLoadNetwork()
    {
        gameData.level = 1;
        
        return gameData.level;
    }
    #endregion
}
