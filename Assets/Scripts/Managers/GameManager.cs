using Installers;
using Settings;
using Signals;
using UnityEngine;
using Zenject;

namespace Managers
{
    public class GameManager : IInitializable, ITickable
    {
        private Constants.GameStates _states, _oldState;
        [Inject] private AsteroidManager _asteroidManager;
        [Inject] private GameSettings _settings;
        [Inject] private GameScriptableSettings _scriptableSettings;
        [Inject] private SignalBus SignalBus;

        public void Initialize()
        {
            SignalBus.Subscribe<GameStartSignal>(OnGameStart);
            // Debug.LogFormat("Width {0}", Width());
            // Debug.LogFormat("Height {0}", Height());
        }

        private void OnGameStart()
        {
            _states = Constants.GameStates.Playing;
        }

        public void Tick()
        {
            return;
            switch (_states)
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
        }

        private void OnHome()
        {
        }

        private void OnPlaying()
        {
            
            _asteroidManager.StartGame();
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