using UnityEngine;

namespace Signals
{
    public class GameStartSignal
    {
    }

    public class GameOverSignal
    {
    }

    public class IncrementScoreSignal
    {
    }

    public class UpdateHealthSignal
    {
        public float MaxHealth;
        public float CurrentHealth;
    }
}