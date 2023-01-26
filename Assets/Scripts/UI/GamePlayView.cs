using Signals;
using UnityEngine;
using Zenject;

namespace UI
{
    public class GamePlayView : View, IInitializable
    {
        public void Initialize()
        {
            SignalBus.Subscribe<GameStartSignal>(OnGameStart);
            SignalBus.Subscribe<GameOverSignal>(OnGameOver);
        }

        private void OnGameStart()
        {
            Show();
        }

        private void OnGameOver()
        {
            Hide();
        }
    }
}