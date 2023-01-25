using Installers;
using Settings;
using Signals;
using UnityEngine;
using Zenject;

namespace Managers
{
    public class GameManager : IInitializable, ITickable
    {
        [Inject] private AsteroidManager _asteroidManager;
        [Inject] private GameSettings _settings;
        [Inject] private GameScriptableSettings _scriptableSettings;
        [Inject] private SignalBus _signalBus;


        public int Level { get; private set; }
        public Constants.GameStates OldState { get; private set; }
        public Constants.GameStates CurrentState { get; private set; }

        public void Initialize()
        {
            _signalBus.Subscribe<GameStartSignal>(OnGameStart);
            // Debug.LogFormat("Width {0}", Width());
            // Debug.LogFormat("Height {0}", Height());
        }

        private void OnGameStart()
        {
            Level = 0;
            CurrentState = Constants.GameStates.Playing;
        }

        public void Tick()
        {
            switch (CurrentState)
            {
                case Constants.GameStates.Home:
                    OnHome();
                    break;
                case Constants.GameStates.Playing:
                    OnPlaying();
                    break;
                case Constants.GameStates.GameOver:
                    OnGameOver();
                    break;
            }

            OldState = CurrentState;
        }

        private void OnHome()
        {
        }

        private void OnPlaying()
        {
            if (OldState != CurrentState)
            {
                _asteroidManager.StartGame();
            }
        }

        private void OnGameOver()
        {
        }

        public float Height()
        {
            return _settings.MainCamera.orthographicSize * 2f;
        }

        public float Width()
        {
            var camera = _settings.MainCamera;
            return 2f * camera.aspect * camera.orthographicSize;
        }
    }
}