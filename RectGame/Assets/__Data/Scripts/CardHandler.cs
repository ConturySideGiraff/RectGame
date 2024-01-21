using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public enum CardState
{
    Close,
    Open,
    Rotating,
    Correct,
    Fail
}

public class CardHandler : MonoBehaviour
{
    [Header("[ Prefab ]")] 
    [SerializeField] private CardBehaviour prefab;
    [SerializeField] private GridData grid;

    [Header("[ Debug ]")]
    [SerializeField, ReadOnly] private CardBehaviour prevCard;
    [SerializeField, ReadOnly] private CardBehaviour nowCard;

    private DataManager _dataManager;
    private CardSpawnPool<CardBehaviour> _cardSpawnPool;
    private Transform _cardLayerTr;
    private UIGame _cardUI;
    
    public bool IsCanFlip => nowCard == null;

    private void Awake()
    {
        _cardUI = UIManager.Instance.GetDisplay<UIGame>();
        _dataManager = DataManager.Instance;
        _cardSpawnPool = new CardSpawnPool<CardBehaviour>();
    }

    public async UniTask<GameData> CardAddOnly(int xLen = -1, int yLen = -1, int width = -1, int height = -1, int xSpace = -1, int ySpace = -1)
    {
        // get ui, layer
        _cardLayerTr = _cardUI.Layer(0);
        
        // card count clamp
        var previousXLen = xLen < 2 ? grid.xLen : xLen;
        var previousYLen = yLen < 2 ? grid.yLen : yLen;
        
        _cardUI.ClampCount(previousXLen, previousYLen, grid.xScale, grid.yScale, grid.xSpace, grid.ySpace,
            (xClampLen, yClampLen) =>
            {
                grid.xLen = xClampLen;
                grid.yLen = yClampLen;
            });
        
        Debug.Log($"card count : [{previousXLen},{previousYLen}] => [{grid.xLen},{grid.yLen}]");
        
        // card only spawn, active off, show back sprite
        var spawnCount = grid.CardCount - _cardSpawnPool.Count;
        for (var i = 0; i < spawnCount; i++)
        {
            _ = _cardSpawnPool.AddOnly(_cardLayerTr, prefab);
            await UniTask.Yield();
        }
        
        // card count
        return new GameData(grid.xLen, grid.yLen, grid.xScale, grid.yScale, grid.xSpace, grid.ySpace);
    }

    public void CardInit()
    {
        // card front image pick
        List<int> frontPickSource = new List<int>();

        int totalCount = grid.CardCount;
        int frontCount = _dataManager.CardFrontSpriteList.Count * 2;
        
        int quo = totalCount / frontCount;
        int remain = totalCount % frontCount;

        for (int i = 0; i < quo; i++)
        {
            for (int j = 0; j < 2; j++)
            {
                int[] source = Util.GetNoneOverlapNumbers(_dataManager.CardFrontSpriteList.Count);
                frontPickSource.AddRange(source);
            }
        }
        
        if (remain > 0)
        {
            int pickCount = remain / 2;
            int[] source = Util.GetNoneOverlapNumbers(_dataManager.CardFrontSpriteList.Count, pickCount);

            for (int i = 0; i < 2; i++)
            {
                for (int j = 0; j < pickCount; j++)
                {
                    frontPickSource.Insert(frontPickSource.Count, source[j]);
                }
            }
        }

        // card back image pick
        var backPick = Random.Range(0, _dataManager.CardBackSpriteList.Count);

        // card active on
        var spawnList = _cardSpawnPool.GetSpawnList(grid.CardCount);
        for (var i = 0; i < totalCount; i++)
        {
            var card = spawnList[i];
            var index = frontPickSource[i];
            var frontSprite = _dataManager.CardFrontSpriteList[index];
            var backSprite = _dataManager.CardBackSpriteList[backPick];
            
            card.InitComponent(index, backSprite, frontSprite, OnFlip, OnResult);
            card.VisualStart(true);
        }
        
        // card null
        OnCardNull();
    }
    
    private CardState OnFlip(CardBehaviour card)
    {
        var result = CardState.Open;

        if (prevCard == null)
        {
            prevCard = card;
            
            return result;
        }
        
        nowCard = card;

        result = prevCard.Index == card.Index ? CardState.Correct : CardState.Fail;
      
        return result;
    }

    private void OnResult(CardState state)
    {
        if (prevCard == null)
            return;

        switch (state)
        {
            case CardState.Close:
            case CardState.Open:
            case CardState.Rotating:
                return;
            case CardState.Correct:
                _ = GameManager.Instance.OnWin();
                OnCardNull();
                break;
            case CardState.Fail:
                _ = prevCard.FlipClose();
                _ = nowCard.FlipClose();
                OnCardNull();
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(state), state, null);
        }
    }

    private void OnCardNull()
    {
        prevCard = null;
        nowCard = null;
    }
}