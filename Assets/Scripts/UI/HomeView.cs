using System;
using Signals;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace UI
{
    public class HomeView : View
    {
        [SerializeField] private Button startButton;

        private void Start()
        {
            startButton.onClick.AddListener(OnStart);
        }

        private void OnStart()
        {
            SignalBus.Fire<GameStartSignal>();
            Hide();
        }
    }
}