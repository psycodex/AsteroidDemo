using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using Zenject.Asteroids;

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
        [SerializeField] private LevelSetting levelSetting;

        public WorldSetting World => worldSetting;
        public PlayerSetting Player => playerSetting;
        public BulletSetting Bullet => bulletSetting;
        public LevelSetting Level => levelSetting;
    }

    [Serializable]
    public class WorldSetting
    {
        #region Properties

        public int TargetFps => targetFps;

        #endregion

        #region Fields

        [SerializeField] private int targetFps = 60;

        #endregion

        #region Methods

        #endregion
    }

    [Serializable]
    public class PlayerSetting
    {
        [SerializeField] private float moveSpeed = 10;
        [SerializeField] private float firePerSecond = 1;
        [SerializeField] private int maxFireBullet = 3;
        public float MoveSpeed => moveSpeed;
        public float FirePerSecond => firePerSecond;
        public float MaxFireBullet => maxFireBullet;
    }

    [Serializable]
    public class BulletSetting
    {
        [SerializeField] private float bulletLifeTime = 10;
        [SerializeField] private float bulletSpeed = 10;

        public float BulletLifeTime => bulletLifeTime;
        public float BulletSpeed => bulletSpeed;
    }

    [Serializable]
    public class LevelSetting
    {
        [SerializeField] private List<Level> levels;
        public List<Level> Levels => levels;
    }

    [Serializable]
    public class Level
    {
        [SerializeField] private int startingSpawnAsteroids = 6;
        [SerializeField] private float spawnIntervals = 1;
        [SerializeField] private float minAsteroids = 1;
        [SerializeField] private float maxAsteroids = 10;

        [SerializeField] private float minSpeed = 1;
        [SerializeField] private float maxSpeed = 5;
        public float MinAsteroids => minAsteroids;
        public float MaxAsteroids => maxAsteroids;
        public float MinSpeed => minSpeed;
        public float MaxSpeed => maxSpeed;
        public int StartingSpawnAsteroids => startingSpawnAsteroids;
        public float SpawnIntervals => spawnIntervals;
    }
}