using UnityEngine;
using Zenject;

namespace Managers
{
    public class GameManager : ITickable
    {
        private Constants.GameStates _states;

        public void Tick()
        {
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
        }

        private void OnGameOver()
        {
        }
    }
}