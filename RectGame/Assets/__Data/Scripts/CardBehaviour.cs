using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardBehaviour : Button
{
    private RectTransform _rt;

    public bool ActiveInHierarchy => gameObject.activeInHierarchy;
    
    protected override void Awake()
    {
        _rt = GetComponent<RectTransform>();
    }

    public void GameInit()
    {
        gameObject.SetActive(true);
    }
}
