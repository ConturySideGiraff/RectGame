using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

public partial class GameManager : SingletonManager<GameManager>
{
}

public partial class GameManager
{
   public enum State
   {
      Auth,
      GameWait,
      GameInit,
      Game,
      GameResult,
   }
   
   [SerializeField] private bool isDebug = true;
   [SerializeField] private State state = State.Auth;
   [Space] 
   [SerializeField] private CardSpawnHandler cardSpawnHandler;
   
   public void ChangeState(State newState)
   {
      state = newState;

      switch (state)
      {
         case State.Auth:
            Auth();
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

   private void Auth()
   {
      if(isDebug)
         ChangeState(State.GameWait);
   }

   private async void GameWait()
   {
      await UniTask.WhenAll(
         cardSpawnHandler.GameWait()
         );
      
      // edit
      ChangeState(State.GameInit);
   }

   private async void GameInit()
   {
      await UniTask.WhenAll(
         cardSpawnHandler.GameInit()
         );
       
      
      ChangeState(State.Game);
   }

   private void Game()
   {
      
   }

   private void GameResult()
   {
      
   }
}

public partial class GameManager
{
   private void Awake()
   {
      cardSpawnHandler ??= FindObjectOfType<CardSpawnHandler>();
   }

   private void Start()
   {
      ChangeState(State.Auth);
   }
}
