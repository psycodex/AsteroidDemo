using Signals;
using TMPro;
using UnityEngine;
using Zenject;

namespace UI
{
    public class GamePlayView : View, IInitializable
    {
        [SerializeField] private TextMeshProUGUI scoreText;
        private int _score;

        public void Initialize()
        {
            SignalBus.Subscribe<GameStartSignal>(OnGameStart);
            SignalBus.Subscribe<GameOverSignal>(OnGameOver);
            SignalBus.Subscribe<IncrementScoreSignal>(OnIncrementScore);
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