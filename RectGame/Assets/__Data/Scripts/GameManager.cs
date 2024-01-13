using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEditor;
using UnityEngine;

public partial class GameManager : SingletonManager<GameManager>
{
}

public partial class GameManager
{
   public enum State
   {
      Auth,
      ResourceCheck,
      GameWait,
      GameInit,
      Game,
      GameResult,
   }
   
   [SerializeField] private State state = State.Auth;
 
   private CardSpawnHandler _cardSpawnHandler;
   private VersionHandler _versionHandler;
   
   public void ChangeState(State newState)
   {
      state = newState;

      switch (state)
      {
         case State.Auth:
            Auth();
            break;
         case State.ResourceCheck:
            ResourceCheck();
            break;
         case State.GameWait:
            GameWait();
            break;
         case State.GameInit:
            GameInit();
            break;
         case State.Game:
            Game();
            break;
         case State.GameResult:
            GameResult();
            break;
         default:
            throw new ArgumentOutOfRangeException();
      }
   }

   private async void Auth()
   {
      await UniTask.WhenAll();
      
      ChangeState(State.ResourceCheck);
   }

   private async void ResourceCheck()
   {
      await UniTask.WhenAll();
      
      ChangeState(State.GameWait);
   }

   private async void GameWait()
   {
      var totalCount = await UniTask.WhenAll(_cardSpawnHandler.CardAddOnly());
      
      ChangeState(State.GameInit);
   }

   private async void GameInit()
   {
      var iResultList = await UniTask.WhenAll(_cardSpawnHandler.CardInit(false));
         
      ChangeState(State.Game);
   }

   private async void Game()
   {
      await UniTask.WhenAll();
   }

   private async void GameResult()
   {
      await UniTask.WhenAll();
   }
}

public partial class GameManager
{
   private void Awake()
   {
      _cardSpawnHandler = FindObjectOfType<CardSpawnHandler>();
      _versionHandler = FindObjectOfType<VersionHandler>();
   }

   private void Start()
   {
      ChangeState(State.Auth);
   }
}
