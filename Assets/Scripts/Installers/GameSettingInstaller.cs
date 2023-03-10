using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using Zenject;

namespace Installers
{
    [CreateAssetMenu(fileName = "GameSettingInstaller", menuName = "Installers/Game Settings")]
    public class GameSettingInstaller : ScriptableObjectInstaller<GameSettingInstaller>
    {
        [SerializeField] private GameScriptableSettings settings;

        public override void InstallBindings()
        {
            Container.Bind<GameScriptableSettings>().FromInstance(settings).AsSingle();
        }
    }

    [Serializable]
    public class GameScriptableSettings
    {
        [SerializeField] private WorldSetting worldSetting;
        [SerializeField] private PlayerSetting playerSetting;
        [SerializeField] private BulletSetting bulletSetting;
        [SerializeField] private AsteroidSetting asteroidSetting;
        [SerializeField] private PowerUpSetting powerUpSetting;
        [SerializeField] private LevelSetting levelSetting;

        public WorldSetting World => worldSetting;
        public PlayerSetting Player => playerSetting;
        public BulletSetting Bullet => bulletSetting;
        public AsteroidSetting Asteroid => asteroidSetting;
        public PowerUpSetting PowerUp => powerUpSetting;
        public LevelSetting Level => levelSetting;
    }

    [Serializable]
    public class WorldSetting
    {
        [SerializeField] private int targetFps = 60;
        [SerializeField] private float initialLevelTime = 10f;

        public int TargetFps => targetFps;
        public float InitialLevelTime => initialLevelTime;
    }

    [Serializable]
    public class PlayerSetting
    {
        [SerializeField] private float health = 10;
        [SerializeField] private float moveSpeed = 10;
        [SerializeField] private float angularSpeed = 1;
        [SerializeField] private float firePerSecond = 1;
        [SerializeField] private int maxFireBullet = 3;

        [SerializeField] private Color minHealth = Color.red;
        [SerializeField] private Color maxHealth = Color.green;

        public float Health => health;
        public float MoveSpeed => moveSpeed;
        public float AngularSpeed => angularSpeed;
        public float FirePerSecond => firePerSecond;
        public float MaxFireBullet => maxFireBullet;

        public Color MinHealth => minHealth;

        public Color MaxHealth => maxHealth;
    }

    [Serializable]
    public class BulletSetting
    {
        [SerializeField] private float bulletLifeTime = 5;
        [SerializeField] private float bulletSpeed = 10;
        [SerializeField] private int burstMax = 3;
        [SerializeField] private float burstIntervalMillSec = 0.1f;

        public float BulletLifeTime => bulletLifeTime;
        public float BulletSpeed => bulletSpeed;

        public int BurstMax => burstMax;

        public float BurstIntervalMillSec => burstIntervalMillSec;
    }

    [Serializable]
    public class LevelSetting
    {
        [SerializeField] private List<Level> levels;
        public List<Level> Levels => levels;
    }

    [Serializable]
    public class AsteroidSetting
    {
        [SerializeField] private int startingSpawnAsteroids = 6;

        [SerializeField] private float initialSpawnIntervals = 1;

        [SerializeField] private float minAsteroids = 1;
        [SerializeField] private float maxAsteroids = 10;

        [SerializeField] private float minSpeed = 1;
        [SerializeField] private float maxSpeed = 5;

        [SerializeField] private float scaleMassFactor = 1f;
        [SerializeField] private float minSize;
        [SerializeField] private float maxSize;
        [SerializeField] private int defaultScale = -1;
        [SerializeField] private int maxScale = 1;

        public float MinAsteroids => minAsteroids;
        public float MaxAsteroids => maxAsteroids;
        public float MinSpeed => minSpeed;
        public float MaxSpeed => maxSpeed;
        public int StartingSpawnAsteroids => startingSpawnAsteroids;
        public float InitialSpawnIntervals => initialSpawnIntervals;

        public float MinSize => minSize;
        public float MaxSize => maxSize;

        public int DefaultScale => defaultScale;

        public int MaxScale => maxScale;
        public float ScaleMassFactor => scaleMassFactor;
    }

    [Serializable]
    public class Level
    {
        [SerializeField] private float deltaLevelDuration;

        [SerializeField] private float deltaMinAsteroids;
        [SerializeField] private float deltaSpawnAsteroidsIntervals;
        [SerializeField] private float deltaAsteroidVelocity;
        [SerializeField] private float deltaPowerUpSpawnIntervals;

        public float DeltaLevelDuration => deltaLevelDuration;
        public float DeltaMinAsteroids => deltaMinAsteroids;
        public float DeltaSpawnAsteroidsIntervals => deltaSpawnAsteroidsIntervals;
        public float DeltaAsteroidVelocity => deltaAsteroidVelocity;
        public float DeltaPowerUpSpawnIntervals => deltaPowerUpSpawnIntervals;
    }

    [Serializable]
    public class PowerUpSetting
    {
        [SerializeField] private float spawnDurationInterval;
        [SerializeField] private float spawnPowerUpLiveDuration;
        [SerializeField] private float playerPowerUpLiveDuration;

        public float SpawnDurationInterval => spawnDurationInterval;
        public float SpawnPowerUpLiveDuration => spawnPowerUpLiveDuration;
        public float PlayerPowerUpLiveDuration => playerPowerUpLiveDuration;
    }
}