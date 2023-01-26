using System;
using UnityEngine;

namespace Settings
{
    [Serializable]
    public class GameSettings
    {
        [SerializeField] private Camera mainCamera;

        [Serializable]
        public class ViewPrefabs
        {
            #region Fields

            [SerializeField] private GameObject player;
            [SerializeField] private GameObject play;
            [SerializeField] private GameObject bulletPrefab;
            [SerializeField] private GameObject asteroidPrefab;
            [SerializeField] private GameObject explosionPrefab;

            [SerializeField] private GameObject homeView;
            [SerializeField] private GameObject gamePlayView;
            [SerializeField] private GameObject gameOverView;

            #endregion

            #region Properties

            public GameObject Player => player;

            public GameObject Play => play;
            public GameObject BulletPrefab => bulletPrefab;
            public GameObject AsteroidPrefab => asteroidPrefab;

            public GameObject ExplosionPrefab => explosionPrefab;

            public GameObject HomeView => homeView;

            public GameObject GamePlayView => gamePlayView;
            public GameObject GameOverView => gameOverView;

            #endregion
        }

        #region Fields

        [SerializeField] private ViewPrefabs views;

        #endregion

        #region Properties

        public Camera MainCamera => mainCamera;

        public ViewPrefabs Views => views;

        #endregion
    }
}