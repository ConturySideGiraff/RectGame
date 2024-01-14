using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class CardBehaviour : MonoBehaviour
{
    [SerializeField] 
    private CardState state;
    
    public const float OpenTime = 1.0f;
    public const float CloseTime = 0.5f;
    
    private RectTransform _rt;
    private Button _button;
    private Image _image;

    private Sprite _frontSprite;
    private Sprite _backSprite;

    private Func<CardBehaviour, CardState> _onFlip;
    private Action _onResult;

    public int Index { get; private set; }

    public CardState State => state;
    
    public void Init(int index, Sprite backSprite, Sprite frontSprite, Func<CardBehaviour, CardState> onFlip, Action onResult)
    {
        Index = index;

        _rt ??= GetComponent<RectTransform>();
        _image ??= GetComponent<Image>();
        _button ??= GetComponent<Button>();

        _button.onClick.RemoveListener(FlipOpen);
        _button.onClick.AddListener(FlipOpen);
        
        _backSprite = backSprite;
        _frontSprite = frontSprite;

        _onFlip = onFlip;
        _onResult = onResult;
    }

    public void VisualStart(bool isCloseCard)
    {
        if (isCloseCard)
        {
            _image.sprite = _backSprite;

            state = CardState.Close;
        }

        else
        {
            _image.sprite = _frontSprite;
            
            state = CardState.Open;
        }

        gameObject.SetActive(true);
    }

    private void FlipOpen()
    {
        if (state != CardState.Close || !GameManager.Instance.IsCanFlip)
        {
            return;
        }

        state = CardState.Rotating;
        
        _ = Rotating(false);
    }

    public async UniTask FlipClose()
    {
        await Rotating(true);
    }

    private async UniTask Rotating(bool isCloseCard)
    {
        Quaternion startRot, endRot;
        float halfTime, totalTime, percent;
        Sprite changeSprite;
        CardState changeState;
        
        if (isCloseCard)
        {     
            startRot = Quaternion.Euler(0, 180, 0);
            endRot = Quaternion.identity;
            
            halfTime = CloseTime / 2;
            totalTime = CloseTime;

            changeSprite = _backSprite;
            changeState = CardState.Close;
        }

        else
        {
            startRot = Quaternion.identity;
            endRot = Quaternion.Euler(0, 180, 0);

            halfTime = OpenTime / 2;
            totalTime = OpenTime;

            changeSprite = _frontSprite;
            changeState = _onFlip.Invoke(this);
        }

        for (float t = 0.0f; t <= halfTime; t += Time.deltaTime)
        {
            percent = t / totalTime;
            transform.localRotation = Quaternion.Lerp(startRot, endRot, percent);
            await UniTask.Yield();
        }

        _image.sprite = changeSprite;
        
        for (float t = halfTime; t <= totalTime; t += Time.deltaTime)
        {
            percent = t / totalTime;
            transform.localRotation = Quaternion.Lerp(startRot, endRot, percent);
            await UniTask.Yield();
        }

        state = changeState;
        
        _onResult.Invoke();
    }
}