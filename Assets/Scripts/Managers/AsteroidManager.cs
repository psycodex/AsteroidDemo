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
        [Inject] private WorldManager _worldManager;
        [Inject] private GameManager _gameManager;

        private float _lastSpawnTime;
        private int _activeAsteroids;
        private int _totalLevelAsteroidSpawned;

        public void ResetAndStart()
        {
            ResetAll();
            SpawnInitialAsteroids();
        }

        private void SpawnInitialAsteroids()
        {
            int level = _gameManager.Level;
            for (int i = 0; i < _scriptableSettings.Level.Levels[level].StartingSpawnAsteroids; i++)
            {
                var asteroid = _asteroidsPool.Add();
                GetRandomPositionAndVelocity(asteroid);
                _lastSpawnTime = Time.realtimeSinceStartup;
            }
        }

        private void ResetAll()
        {
            _player.transform.position = Vector2.zero;
            _bulletsPool.RemoveAll();
            _asteroidsPool.RemoveAll();
        }

        public void GetRandomPositionAndVelocity(Asteroid asteroid)
        {
            int level = _gameManager.Level;
            var minSpeed = _scriptableSettings.Level.Levels[level].MinSpeed;
            var maxSpeed = _scriptableSettings.Level.Levels[level].MaxSpeed;

            var x = Random.Range(-_worldManager.Width, _worldManager.Width);
            var y = Random.Range(-_worldManager.Height, _worldManager.Height);

            var position = new Vector3(x, y, 0);
            // asteroid.transform.position = position;

            var randomDirection = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), 0);
            var dir = randomDirection.normalized;

            var velocity = Random.Range(minSpeed, maxSpeed) * dir;
            // var velocity = Vector3.zero;
            // asteroid.Rigidbody2D.velocity = velocity;

            asteroid.SetMeta(position, velocity, 0);
        }


        public void OnPlaying()
        {
            return;
            Level level = _scriptableSettings.Level.Levels[_gameManager.Level];
            if (Time.realtimeSinceStartup - _lastSpawnTime > level.SpawnIntervals &&
                _activeAsteroids < level.MaxAsteroids)
            {
                var asteroid = _asteroidsPool.Add();
                GetRandomPositionAndVelocity(asteroid);
                _lastSpawnTime = Time.realtimeSinceStartup;
            }
        }
    }
}