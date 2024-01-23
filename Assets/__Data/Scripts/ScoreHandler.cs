using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class ScoreHandler : MonoBehaviour
{
    [Header("[ Option ]")] 
    [SerializeField] private float reduceValue = 10.0f;
    
    [Header("[ Debug ]")]
    [SerializeField, ReadOnly] private float initScore;
    [SerializeField, ReadOnly] private float currentScore;

    private UIBanner _uiBanner;
    private Action<float> _onDataAction;
    private Action _onWinAction;
    private bool _isReduce;
    
    public float Init (float score, Action<float> onDataAction, Action onWinAction)
    {
        initScore = score;
        currentScore = score;

        _onDataAction = onDataAction;
        _onWinAction = onWinAction;
        
        _uiBanner.ScoreUpdate(currentScore);

        return currentScore;
    }

    public void SetReduce(bool isReduce) => _isReduce = isReduce;

    private void Awake()
    {
        _uiBanner = UIManager.Instance.GetDisplay<UIBanner>();
    }

    private void Update()
    {
        if(!_isReduce) return;

        currentScore = Mathf.Clamp(currentScore - Time.deltaTime * DataManager.Instance.Level * reduceValue , 0, initScore) ;

        _onDataAction.Invoke(currentScore);
        _uiBanner.ScoreUpdate(currentScore);
        
        if (currentScore > 0) return;
        
        _onWinAction.Invoke();
        
        _isReduce = false;
    }
}
