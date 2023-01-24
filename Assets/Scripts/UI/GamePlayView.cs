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
        }

        private void OnGameStart()
        {
            Show();
        }
    }
}