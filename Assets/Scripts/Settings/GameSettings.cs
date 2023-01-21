using System;
using UnityEngine;

namespace Settings
{
    [Serializable]
    public class GameSettings
    {
        [Serializable]
        public class ViewPrefabs
        {
            #region Fields

            [SerializeField] private GameObject privacyView;

            #endregion

            #region Properties

            public GameObject PrivacyView => privacyView;

            #endregion
        }

        #region Fields

        [SerializeField] private Camera mainCamera;

        [SerializeField] private ViewPrefabs views;

        #endregion

        #region Properties

        public Camera MainCamera => mainCamera;

        public ViewPrefabs Views => views;

        #endregion
    }
}