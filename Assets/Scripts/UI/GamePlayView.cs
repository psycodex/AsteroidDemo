using System;
using Installers;
using Signals;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace UI
{
    public class GamePlayView : View, IInitializable, IDisposable
    {
        [SerializeField] private TextMeshProUGUI scoreText;
        [SerializeField] private Slider healthSlider;
        [SerializeField] private Image fillImage;
        private int _score;

        [Inject] private GameScriptableSettings _scriptableSettings;

        public void Initialize()
        {
            SignalBus.Subscribe<GameStartSignal>(OnGameStart);
            SignalBus.Subscribe<GameOverSignal>(OnGameOver);
            SignalBus.Subscribe<IncrementScoreSignal>(OnIncrementScore);
            SignalBus.Subscribe<UpdateHealthSignal>(OnUpdateHealth);
        }

        public void Dispose()
        {
            SignalBus.TryUnsubscribe<GameStartSignal>(OnGameStart);
            SignalBus.TryUnsubscribe<GameOverSignal>(OnGameOver);
            SignalBus.TryUnsubscribe<IncrementScoreSignal>(OnIncrementScore);
            SignalBus.TryUnsubscribe<UpdateHealthSignal>(OnUpdateHealth);
        }

        private void OnUpdateHealth(UpdateHealthSignal healthSignal)
        {
            healthSlider.maxValue = healthSignal.MaxHealth;
            fillImage.color = Color.Lerp(_scriptableSettings.Player.MinHealth, _scriptableSettings.Player.MaxHealth,
                healthSignal.CurrentHealth / healthSignal.MaxHealth);
            healthSlider.value = healthSignal.CurrentHealth;
        }

        private void OnIncrementScore()
        {
            _score++;
            UpdateText();
        }

        private void UpdateText()
        {
            scoreText.SetText($"{_score}");
        }

        private void OnGameStart()
        {
            _score = 0;
            UpdateText();
            Show();
        }

        private void OnGameOver()
        {
            Hide();
        }
    }
}