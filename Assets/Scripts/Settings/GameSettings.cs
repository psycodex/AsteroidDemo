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

            #endregion

            #region Properties

            public GameObject Player => player;

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