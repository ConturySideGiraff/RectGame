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
      GameSettingInit,
      GameInit,
      GameStartWait,
      Game,
      GamePause,
      GameResult,
   }

   // Component
   private CardHandler _cardHandler;
   private ScoreHandler _scoreHandler;
   private VersionHandler _versionHandler;
   
   // Instance
   private DataManager _dataManager;

   private void Awake()
   {
      _cardHandler = FindObjectOfType<CardHandler>();
      _scoreHandler = FindObjectOfType<ScoreHandler>();
      _versionHandler = FindObjectOfType<VersionHandler>();

      _dataManager = DataManager.Instance;
   }

   private void Start()
   {
      ChangeState(this, GameState.Auth);
   }
}

public partial class GameManager
{
   [Header("[ Local ]")] 
   [SerializeField] private bool isNetwork;
   public bool IsNetwork => isNetwork;

   
   [Header("[ Debug ]")]
   [SerializeField, ReadOnly] private GameState state = GameState.Auth;

   public GameState State => state;
   
   #region ChangeState
   public void ChangeState(Component sender, GameState newGameState)
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
         case GameState.GameSettingInit:
            GameSettingInit();
            break;
         case GameState.GameInit:
            GameInit();
            break;
         case GameState.GameStartWait:
            GameStartWait();
            break;
         case GameState.Game:
            Game();
            break;
         case GameState.GamePause:
            GamePause();
            break;
         case GameState.GameResult:
            GameResult();
            break;
         default:
            throw new ArgumentOutOfRangeException();
      }
   }
   #endregion

   private void Auth()
   {
      ChangeState(this, GameState.ResourceCheck);
   }

   private void ResourceCheck()
   {
      ChangeState(this, GameState.GameSettingInit);
   }

   private async void GameSettingInit()
   {
      _ = _dataManager.LevelLoad();
      
      var gameData = await _cardHandler.CardAddOnly();
      _ = _dataManager.Init(gameData);
      
      ChangeState(this, GameState.GameInit);
   }

   private void GameInit()
   {
      UIManager.Instance.OffPopupAll();
      
      _ = _scoreHandler.Init(_dataManager.InitScore, _dataManager.OnScoreUpdate, OnLose);
      _cardHandler.CardInit();
      _dataManager.Reset();

      ChangeState(this, GameState.GameStartWait);
   }

   private async void GameStartWait()
   {
      Time.timeScale = 1.0f;

      var uiGameWait = UIManager.Instance.GetPopup<UIGameWait>();

      await uiGameWait.CountDown();
      
      ChangeState(this, GameState.Game);
   }

   private void Game()
   {
      Time.timeScale = 1.0f;
      
      UIManager.Instance.OffPopupAll();
      
      _scoreHandler.SetReduce(true);
   }

   private void GamePause()
   {
      Time.timeScale = 0.0f;

      _scoreHandler.SetReduce(false);
   }

   private void GameResult()
   {
      UIManager.Instance.OffPopupAll();
      
      _scoreHandler.SetReduce(false);

      var uiResult = UIManager.Instance.GetPopup<UIResult>();
      var data = _dataManager.GetData(this);

      if (_isWin)
      {
         Win();
         uiResult.Win(ref data);
      }

      else
      {
         Lose();
         uiResult.Lose(ref data);
      }
      
      uiResult.On();
   }
}

public partial class GameManager
{
   public bool IsCanFlip => _cardHandler.IsCanFlip;

   private bool _isWin;
   
   public bool OnWin()
   {
      _isWin = _dataManager.OnCorrect();

      if (_isWin) ChangeState(this, GameState.GameResult);

      return _isWin;
   }

   // ReSharper disable Unity.PerformanceAnalysis
   private void OnLose()
   {
      _isWin = false;
      
      ChangeState(this, GameState.GameResult);
   }

   private void Win()
   {
      
   }

   private void Lose()
   {
      
   }
}

public partial class GameManager
{

}