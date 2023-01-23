using Installers;
using Settings;
using UnityEngine;
using Zenject;

namespace Managers
{
    public class AsteroidManager
    {
        [Inject] private Player _player;
        [Inject] private Asteroid.AsteroidsPool _asteroidsPool;
        [Inject] private Bullet.BulletsPool _bulletsPool;
        [Inject] private GameSettings _settings;
        [Inject] private GameScriptableSettings _scriptableSettings;

        public void StartGame()
        {
            ResetAll();
            SpawnInitialAsteroids();
        }

        private void SpawnInitialAsteroids()
        {
            for (int i = 0; i < _scriptableSettings.Asteroid.StartingSpawnAsteroids; i++)
            {
                var asteroid =  _asteroidsPool.Add();
                // asteroid.transform.position = ;
                // asteroid.Rigidbody2D.velocity = ;
            }
        }

        private void ResetAll()
        {
            _player.transform.position = Vector2.zero;
            _bulletsPool.RemoveAll();
            _asteroidsPool.RemoveAll();
        }
    }
}