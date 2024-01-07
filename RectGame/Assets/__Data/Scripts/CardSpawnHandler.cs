using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Serialization;

public class CardSpawnHandler : MonoBehaviour
{
    /* COUNT */
    [SerializeField] private int width = 200;
    [SerializeField] private int height = 200;
    [SerializeField] private int xLen = 3;
    [SerializeField] private int yLen = 4;
    
    private int TotalCount => xLen * yLen;

    [SerializeField] private CardSpawnPool<CardBehaviour> cardSpawnPool;
    [SerializeField] private List<Sprite> cardBackSpriteList = new List<Sprite>();
    [SerializeField] private List<Sprite> cardFrontSpriteList = new List<Sprite>();

    private UIPageGame _cardUI;
    private Transform _cardLayerTr;

    public async UniTask<int> CardAddOnly()
    {
        // get ui, layer
        _cardUI ??= UIManager.Instance.GetPopup<UIPageGame>();
        _cardLayerTr = _cardUI.Layer(0);
        
        // card count clamp
        _cardUI.GameWait(width, height, xLen, yLen,
            (xClampLen, yClampLen) =>
            {
                xLen = xClampLen;
                yLen = yClampLen;
            });
        
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

    public UniTask<int> CardInit()
    {
        // card active on
        var spawnList = cardSpawnPool.GetSpawnList(TotalCount);
        foreach (var card in spawnList)
        {
            card.Init(cardBackSpriteList[0]);
            card.VisualStart();
        }
        
        // sprite
        return UniTask.FromResult<int>(TotalCount);
    }
}