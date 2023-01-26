using Signals;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace UI
{
    public class GameOverView : View, IInitializable
    {
        [SerializeField] private Button restartButton;

        public void Initialize()
        {
            SignalBus.Subscribe<GameStartSignal>(OnGameStart);
            SignalBus.Subscribe<GameOverSignal>(OnGameOver);
        }

        private void Start()
        {
            restartButton.onClick.AddListener(OnRestart);
        }

        private void OnRestart()
        {
            SignalBus.Fire<GameStartSignal>();
            Hide();
        }

        private void OnGameStart()
        {
            Hide();
        }

        private void OnGameOver()
        {
            Show();
        }
    }
}