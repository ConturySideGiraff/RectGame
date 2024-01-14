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
}

public partial class GameManager
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
   
   [Header("[ Debug ]")]
   [SerializeField] private GameState state = GameState.Auth;
   [SerializeField] private int currentCount;
   [SerializeField] private int targetCount;
   
   private CardHandler _cardHandler;
   private VersionHandler _versionHandler;
   
   //
   
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
      await UniTask.WhenAll();
      
      ChangeState(GameState.ResourceCheck);
   }

   private async void ResourceCheck()
   {
      await UniTask.WhenAll();
      
      ChangeState(GameState.GameWait);
   }

   private async void GameWait()
   {
      var cardCount = await UniTask.WhenAll(_cardHandler.CardAddOnly());

      targetCount = cardCount[0] / 2;
      
      ChangeState(GameState.GameInit);
   }

   private async void GameInit()
   {
      _cardHandler.CardInit();

      currentCount = 0;
      
      ChangeState(GameState.Game);
   }

   private void Game()
   {
      
   }
   
   private async void GameResult()
   {
      await UniTask.WhenAll();
   }
}

public partial class GameManager
{
   public bool IsCanFlip => _cardHandler.IsCanFlip;

   public void OnCorrect()
   {
      currentCount += 1;
   }
}

public partial class GameManager
{
   private void Awake()
   {
      _cardHandler = FindObjectOfType<CardHandler>();
      _versionHandler = FindObjectOfType<VersionHandler>();
   }

   private void Start()
   {
      ChangeState(GameState.Auth);
   }
}
