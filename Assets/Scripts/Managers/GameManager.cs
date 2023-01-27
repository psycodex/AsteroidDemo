using System;
using System.Collections;
using Installers;
using Signals;
using UnityEngine;
using Zenject;

namespace Managers
{
    public class GameManager : IInitializable, IDisposable
    {
        [Inject] private GameScriptableSettings _scriptableSettings;
        [Inject] private AsteroidManager _asteroidManager;
        [Inject] private PowerUpManager _powerUpManager;

        [Inject] private SignalBus _signalBus;
        [Inject] private Play _play;
        [Inject] private PlayerHandler _playerHandler;

        private float _lastLevelTime;
        private int _level;

        public int Level
        {
            get => _level;
            private set
            {
                if (value < _scriptableSettings.Level.Levels.Count)
                {
                    _level = value;
                }
            }
        }

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
            _lastLevelTime = Time.realtimeSinceStartup;
            CurrentState = Constants.GameStates.Playing;
            _playerHandler.Reset();
            _asteroidManager.ResetAndStart();
            _play.StartCoroutine(GamePlay());
        }

        private IEnumerator GamePlay()
        {
            while (CurrentState == Constants.GameStates.Playing)
            {
                var levelSetting = _scriptableSettings.Level.Levels[Level];
                if (Time.realtimeSinceStartup - _lastLevelTime >
                    _scriptableSettings.World.InitialLevelTime + levelSetting.DeltaLevelDuration)
                {
                    Level++;
                }

                _asteroidManager.OnPlaying();
                _powerUpManager.OnPlaying();
                yield return new WaitForEndOfFrame();
            }
        }

        private void OnGameOverSignal()
        {
            CurrentState = Constants.GameStates.GameOver;
        }
    }
}