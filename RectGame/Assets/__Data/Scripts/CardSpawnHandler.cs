using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class CardSpawnHandler : MonoBehaviour
{
    /* COUNT */
    [SerializeField] private int width = 200;
    [SerializeField] private int height = 200;
    [SerializeField] private int xLen = 3;
    [SerializeField] private int yLen = 4;
    
    private int TotalCount => xLen * yLen;

    /* CARD */
    [SerializeField] private CardSpawnPool<CardBehaviour> cardSpawnPool;
    [SerializeField] private List<Sprite> cardSpriteList = new List<Sprite>();

    private UIPageGame _cardUI;
    private Transform _cardLayerTr;

    /* STATE */
    public async UniTask GameWait()
    {
        // get ui
        _cardUI ??= UIManager.Instance.GetPopup<UIPageGame>();
        _cardLayerTr = _cardUI.Layer(0);
        
        // card count
        _cardUI.GameWait(width, height, xLen, yLen,
            (xClampLen, yClampLen) =>
            {
                xLen = xClampLen;
                yLen = yClampLen;
            });
        
        // card spawn
        var spawnCount = TotalCount - cardSpawnPool.Count;
        for (var i = 0; i < spawnCount; i++)
        {
            _ = cardSpawnPool.Add(_cardLayerTr);
            await UniTask.Yield();
        }
    }

    public async UniTask GameInit()
    {
        // card
        for (var i = 0; i < TotalCount; i++)
        {
            var card = cardSpawnPool.Spawn(_cardLayerTr);
            card.GameInit();
            
            await UniTask.Yield();
        }
    }
}