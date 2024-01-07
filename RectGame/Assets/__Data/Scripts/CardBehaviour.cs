using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class CardBehaviour : MonoBehaviour
{
    private const float OpenTime = 1.0f;
    private const float CloseTime = 0.5f;
    
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

    public void Init(Sprite backSprite)
    {
        _backSprite = backSprite;
    }

    public void VisualStart()
    {
        _image.sprite = _backSprite;
        
        gameObject.SetActive(true);

    }

    public void VisualStop()
    {
        gameObject.SetActive(false);
    }
}