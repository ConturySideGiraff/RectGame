using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class CardBehaviour : MonoBehaviour, ICardResult
{
    private const float OpenTime = 1.0f;
    private const float CloseTime = 0.5f;
    
    [SerializeField] private CardState state;

    private RectTransform _rt;
    private Button _button;
    private Image _image;

    private Sprite _frontSprite;
    private Sprite _backSprite;

    
    private void Awake()
    {
        _image = GetComponent<Image>();
        _button = GetComponent<Button>();
        _rt = GetComponent<RectTransform>();
    }

    // func
    public ICardResult Init(Sprite backSprite, Sprite frontSprite)
    {
        _backSprite = backSprite;
        _frontSprite = frontSprite;

        return this;
    }
    
    public void VisualStart(bool isCloseCard)
    {
        _image.sprite = isCloseCard ? _backSprite : _frontSprite;
        
        gameObject.SetActive(true);
    }

    public void VisualEnd()
    {
        gameObject.SetActive(false);
    }

    // interface
    public void OnResult()
    {
        
    }

    public void OnCorrect()
    {
       
    }

    public void OnFail()
    {
        
    }
}