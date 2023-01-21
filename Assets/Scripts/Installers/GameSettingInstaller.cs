using System;
using UnityEngine;
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

        public WorldSetting World => worldSetting;
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
}