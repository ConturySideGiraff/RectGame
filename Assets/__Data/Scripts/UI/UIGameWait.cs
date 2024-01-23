using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class UIGameWait : UIPopup
{
    [SerializeField] private Text countText;

    private const float Duration = 3.0f;

    private float _countTime = 0;
    private CancellationTokenSource _cts = null;

    public async UniTask CountDown()
    {
        _cts?.Cancel();
        _cts = new CancellationTokenSource();
        
        await OnCountDown();
    }

    private async UniTask OnCountDown()
    {
        On();
        
        for (float t = Duration; t >= 0.0f; t -= Time.deltaTime)
        {
            if (_cts.Token.IsCancellationRequested)
                return;
            
            _countTime = t;

            await UniTask.Yield(PlayerLoopTiming.Update, _cts.Token);
        }
    }

    private void Update()
    {
        countText.text = $"{_countTime:0.00}";
    }
}
