using System;
using Installers;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

namespace Managers
{
    public class PowerUpManager : IInitializable, IDisposable
    {
        [Inject] private GameScriptableSettings _scriptableSettings;
        [Inject] private WorldManager _worldManager;
        [Inject] private PowerUp.PowerUpPool _powerUpPool;
        private float _lastSpawnTime;

        public void Initialize()
        {
            _lastSpawnTime = Time.realtimeSinceStartup;
        }

        public void OnPlaying()
        {
            var powerUpSetting = _scriptableSettings.PowerUp;
            if (Time.realtimeSinceStartup - _lastSpawnTime > powerUpSetting.SpawnDurationInterval)
            {
                var powerUp = _powerUpPool.Add(Constants.PowerUpsType.Shield);
                _lastSpawnTime = Time.realtimeSinceStartup;
                GetRandomPositionAndVelocity(powerUp);
            }
        }

        public void GetRandomPositionAndVelocity(PowerUp powerUp)
        {
            var minSpeed = _scriptableSettings.Asteroid.MinSpeed;
            var maxSpeed = _scriptableSettings.Asteroid.MaxSpeed;

            var x = Random.Range(-_worldManager.Width, _worldManager.Width);
            var y = Random.Range(-_worldManager.Height, _worldManager.Height);

            var position = new Vector3(x, y, 0);
            var randomDirection = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), 0);
            var dir = randomDirection.normalized;

            var velocity = Random.Range(minSpeed, maxSpeed) * dir;

            powerUp.transform.position = position;
            // powerUp.Rigidbody2D.velocity = velocity;
        }


        public void Dispose()
        {
        }
    }
}