using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;

public partial class GameManager : SingletonManager<GameManager>
{
   public enum GameState
   {
      Auth,
      ResourceCheck,
      GameWait,
      GameInit,
      Game,
      GameResult,
   }

   private CardHandler _cardHandler;
   private DataHandler _dataHandler;
   private ScoreHandler _scoreHandler;
   private VersionHandler _versionHandler;

   private void Awake()
   {
      _cardHandler = FindObjectOfType<CardHandler>();
      _dataHandler = FindObjectOfType<DataHandler>();
      _scoreHandler = FindObjectOfType<ScoreHandler>();
      _versionHandler = FindObjectOfType<VersionHandler>();
   }

   private void Start()
   {
      ChangeState(GameState.Auth);
   }
}

public partial class GameManager
{
   [Header("[ Debug ]")]
   [SerializeField] private GameState state = GameState.Auth;
   
   private void ChangeState(GameState newGameState)
   {
      state = newGameState;

      switch (state)
      {
         case GameState.Auth:
            Auth();
            break;
         case GameState.ResourceCheck:
            ResourceCheck();
            break;
         case GameState.GameWait:
            GameWait();
            break;
         case GameState.GameInit:
            GameInit();
            break;
         case GameState.Game:
            Game();
            break;
         case GameState.GameResult:
            GameResult();
            break;
         default:
            throw new ArgumentOutOfRangeException();
      }
   }

   private async void Auth()
   {
      UIManager.Instance.OffPopupAll();
      
      ChangeState(GameState.ResourceCheck);
   }

   private async void ResourceCheck()
   {
      ChangeState(GameState.GameWait);
   }

   private async void GameWait()
   {
      var gameData = await _cardHandler.CardAddOnly();
      _ = _dataHandler.Init(gameData);
      _ = _scoreHandler.Init(gameData.score, _dataHandler.OnScoreUpdate, OnLose);
      
      ChangeState(GameState.GameInit);
   }

   private async void GameInit()
   {
      _cardHandler.CardInit();
      _dataHandler.Reset();
      
      ChangeState(GameState.Game);
   }

   private void Game()
   {
      _scoreHandler.SetReduce(true);
   }
   
   private async void GameResult()
   {
      _scoreHandler.SetReduce(false);
      
      if(_isWin)
         Win();
      else
         Lose();

      var uiResult = UIManager.Instance.GetPopup<UIResult>();
      uiResult.On();
   }
}

public partial class GameManager
{
   public bool IsCanFlip => _cardHandler.IsCanFlip;

   private bool _isWin;
   
   public bool OnWin()
   {
      _isWin = _dataHandler.OnCorrect();

      if (_isWin) ChangeState(GameState.GameResult);

      return _isWin;
   }

   // ReSharper disable Unity.PerformanceAnalysis
   private void OnLose()
   {
      _isWin = false;
      
      ChangeState(GameState.GameResult);
   }

   private async void Win()
   {
      Debug.Log("win");
   }

   private async void Lose()
   {
      Debug.Log("lose");
   }
}

public partial class GameManager
{

}