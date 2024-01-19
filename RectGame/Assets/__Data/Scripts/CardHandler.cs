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
    [SerializeField, ReadOnly] private CardBehaviour prevCard;
    [SerializeField, ReadOnly] private CardBehaviour nowCard;
    
    [FormerlySerializedAs("xLen")]
    [Header("[ Data Option ]")]
    [SerializeField] private int defaultXLen = 3;
    [SerializeField] private int defaultYLen = 4;
    [Space]
    [SerializeField] private int defaultWidth = 150;
    [SerializeField] private int defaultHeight = 300;
    [Space] 
    [SerializeField] private int defaultXSpace = 15;
    [SerializeField] private int defaultYSpace = 30;
    [Space]
    [SerializeField] private CardSpawnPool<CardBehaviour> cardSpawnPool;
    [SerializeField] private List<Sprite> cardBackSpriteList = new List<Sprite>();
    [SerializeField] private List<Sprite> cardFrontSpriteList = new List<Sprite>();
    
    //

    private int _xLen;
    private int _yLen;
    
    //
    
    private UIGame _cardUI;
    private Transform _cardLayerTr;

    //
    
    public bool IsCanFlip => nowCard == null;
    private int TotalCount => _xLen * _yLen;
    
    //

    private void Awake()
    {
        _cardUI = UIManager.Instance.GetDisplay<UIGame>();
    }

    public async UniTask<GameData> CardAddOnly(int xLen = -1, int yLen = -1)
    {
        // get ui, layer
        _cardLayerTr = _cardUI.Layer(0);
        
        // card count clamp
        var previousXLen = xLen < 2 ? defaultXLen : xLen;
        var previousYLen = yLen < 2 ? defaultYLen : yLen;
        
        _cardUI.ClampCount(previousXLen, previousYLen, defaultWidth, defaultHeight, defaultXSpace, defaultYSpace,
            (xClampLen, yClampLen) =>
            {
                _xLen = xClampLen;
                _yLen = yClampLen;
            });
        
        Debug.Log($"card count : [{previousXLen},{previousYLen}] => [{_xLen},{_yLen}][{_xLen * _yLen}]");
        
        // card only spawn, active off, show back sprite
        var spawnCount = TotalCount - cardSpawnPool.Count;
        for (var i = 0; i < spawnCount; i++)
        {
            _ = cardSpawnPool.AddOnly(_cardLayerTr);
            await UniTask.Yield();
        }
        
        // card count
        return new GameData(_xLen, _yLen);
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