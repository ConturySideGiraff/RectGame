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
    [Header("[ Debug ]")]
    [SerializeField] private CardBehaviour prevCard;
    [SerializeField] private CardBehaviour nowCard;
    
    [Header("[ Data Option ]")]
    [SerializeField] private int xLen = 3;
    [SerializeField] private int yLen = 4;
    [Space]
    [SerializeField] private int width = 150;
    [SerializeField] private int height = 300;
    [Space] 
    [SerializeField] private int xSpace = 15;
    [SerializeField] private int ySpace = 30;
    [Space]
    [SerializeField] private CardSpawnPool<CardBehaviour> cardSpawnPool;
    [SerializeField] private List<Sprite> cardBackSpriteList = new List<Sprite>();
    [SerializeField] private List<Sprite> cardFrontSpriteList = new List<Sprite>();
    
    //
    
    private UIPageGame _cardUI;
    private Transform _cardLayerTr;

    //
    
    public bool IsCanFlip =>  nowCard == null;
    private int TotalCount => xLen * yLen;
    
    //
    
    public async UniTask<int> CardAddOnly()
    {
        // get ui, layer
        _cardUI ??= UIManager.Instance.GetPopup<UIPageGame>();
        _cardLayerTr = _cardUI.Layer(0);
        
        // card count clamp
        var previousXLen = xLen;
        var previousYLen = yLen;
        
        _cardUI.GameWait(xLen, yLen, width, height, xSpace, ySpace,
            (xClampLen, yClampLen) =>
            {
                xLen = xClampLen;
                yLen = yClampLen;
            });

        Debug.Log($"card count : [{previousXLen},{previousYLen}] => [{xLen},{yLen}] = {xLen * yLen}");
        
        // card only spawn, active off, show back sprite
        var spawnCount = TotalCount - cardSpawnPool.Count;
        for (var i = 0; i < spawnCount; i++)
        {
            _ = cardSpawnPool.AddOnly(_cardLayerTr);
            await UniTask.Yield();
        }
        
        // card count
        return TotalCount;
    }

    public void CardInit()
    {
        // card front image pick
        List<int> frontPickSource = new List<int>();

        int totalCount = TotalCount;
        int frontCount = cardFrontSpriteList.Count * 2;
        
        int quo = totalCount / frontCount;
        int remain = totalCount % frontCount;

        for (int i = 0; i < quo; i++)
        {
            for (int j = 0; j < 2; j++)
            {
                int[] source = Util.GetNoneOverlapNumbers(cardFrontSpriteList.Count);
                frontPickSource.AddRange(source);
            }
        }
        
        if (remain > 0)
        {
            int pickCount = remain / 2;
            int[] source = Util.GetNoneOverlapNumbers(cardFrontSpriteList.Count, pickCount);

            for (int i = 0; i < 2; i++)
            {
                for (int j = 0; j < pickCount; j++)
                {
                    frontPickSource.Insert(frontPickSource.Count, source[j]);
                }
            }
        }

        // card back image pick
        var backPick = Random.Range(0, cardBackSpriteList.Count);

        // card active on
        var spawnList = cardSpawnPool.GetSpawnList(TotalCount);
        for (var i = 0; i < totalCount; i++)
        {
            var card = spawnList[i];
            var index = frontPickSource[i];
            var frontSprite = cardFrontSpriteList[index];
            var backSprite = cardBackSpriteList[backPick];
            
            card.Init(index, backSprite, frontSprite, OnFlip, OnResult);
            card.VisualStart(true);
        }
    }
    
    private CardState OnFlip(CardBehaviour nowCard)
    {
        var result = CardState.Open;

        if (prevCard == null)
        {
            prevCard = nowCard;
            
            return result;
        }
        
        this.nowCard = nowCard;

        result = prevCard.Index == nowCard.Index ? CardState.Correct : CardState.Fail;
      
        return result;
    }

    private void OnResult()
    {
        if (prevCard == null || nowCard == null)
        {
            return;
        }

        if (prevCard.State == CardState.Rotating || nowCard.State == CardState.Rotating)
        {
            return;
        }

        if (nowCard.State == CardState.Fail || prevCard.State == CardState.Fail)
        {
            _ = prevCard.FlipClose();
            _ = nowCard.FlipClose();
        }
        
        prevCard = null;
        nowCard = null;
    }
}