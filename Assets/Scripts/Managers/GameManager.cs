using System;
using System.Collections;
using System.Collections.Generic;
using Installers;
using Settings;
using Signals;
using UI;
using UnityEngine;
using Zenject;

namespace Managers
{
    public class GameManager : IInitializable, IDisposable //, ITickable
    {
        [Inject] private AsteroidManager _asteroidManager;
        [Inject] private GameSettings _settings;

        [Inject] private GameScriptableSettings _scriptableSettings;
        [Inject] private SignalBus _signalBus;
        [Inject] private GamePlayView _playView;
        [Inject] private Play _play;
        [Inject] private PlayerHandler _playerHandler;

        public int Level { get; private set; }
        public Constants.GameStates OldState { get; private set; }
        public Constants.GameStates CurrentState { get; private set; }

        public void Initialize()
        {
            _signalBus.Subscribe<GameStartSignal>(OnGameStartSignal);
            _signalBus.Subscribe<GameOverSignal>(OnGameOverSignal);
        }

        public void Dispose()
        {
            _signalBus.Unsubscribe<GameStartSignal>(OnGameStartSignal);
            _signalBus.Unsubscribe<GameOverSignal>(OnGameOverSignal);
        }

        private void OnGameStartSignal()
        {
            Level = 0;
            CurrentState = Constants.GameStates.Playing;
            _playerHandler.Reset();
            _asteroidManager.ResetAndStart();
            _play.StartCoroutine(GamePlay());
        }

        private IEnumerator GamePlay()
        {
            while (CurrentState == Constants.GameStates.Playing)
            {
                _asteroidManager.OnPlaying();
                yield return new WaitForEndOfFrame();
            }
        }

        private void OnGameOverSignal()
        {
            CurrentState = Constants.GameStates.GameOver;
        }

        // public void Tick()
        // {
        //     switch (CurrentState)
        //     {
        //         case Constants.GameStates.Home:
        //             OnHome();
        //             break;
        //         case Constants.GameStates.Playing:
        //             OnPlaying();
        //             break;
        //         case Constants.GameStates.GameOver:
        //             OnGameOver();
        //             break;
        //     }
        //
        //     OldState = CurrentState;
        // }

        // private void OnHome()
        // {
        // }
        //
        // private void OnPlaying()
        // {
        //     _asteroidManager.OnPlaying();
        // }
        //
        // private void OnGameOver()
        // {
        // }
    }
}