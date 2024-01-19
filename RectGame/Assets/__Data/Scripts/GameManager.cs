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
      ChangeState(this, GameState.Auth);
   }
}

public partial class GameManager
{
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
            GameWait();
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
      // TODO 1. 네트워크 연결 시 해당 정보를 기반으로 카드 개수를 산정
      // TODO 2. 해당 개수를 파라미터로 넘겨주어 값을 확인
      
      var gameData = await _cardHandler.CardAddOnly();
      _ = _dataHandler.Init(gameData);
      
      ChangeState(this, GameState.GameInit);
   }

   private void GameInit()
   {
      UIManager.Instance.OffPopupAll();
      
      _ = _scoreHandler.Init(_dataHandler.InitScore, _dataHandler.OnScoreUpdate, OnLose);
      _cardHandler.CardInit();
      _dataHandler.Reset();

      ChangeState(this, GameState.GameStartWait);
   }

   private void GameWait()
   {
      var uiGameWait = UIManager.Instance.GetPopup<UIGameWait>();

      // await uiGameWait.OnGameWait();
      
      ChangeState(this, GameState.Game);
   }

   private void Game()
   {
      UIManager.Instance.OffPopupAll();

      Time.timeScale = 1.0f;
      
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
      var data = _dataHandler.GetData(this);

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
      _isWin = _dataHandler.OnCorrect();

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