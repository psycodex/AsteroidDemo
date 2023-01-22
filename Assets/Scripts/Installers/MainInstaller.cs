using Settings;
using UnityEngine;
using Zenject;

namespace Installers
{
    public class MainInstaller : MonoInstaller<MainInstaller>
    {
        [SerializeField] private GameSettings settings;

        // [Inject] private GameScriptableSettings gameScriptableSettings;

        public override void InstallBindings()
        {
            BindSettings();
            BindSignals();
            BindGame();
        }

        private void BindSettings()
        {
            Container.Bind<GameSettings>().FromInstance(settings).AsSingle();
        }

        private void BindGame()
        {
            Container.BindInterfacesAndSelfTo<Player>().FromComponentInHierarchy(settings.Views.Player).AsSingle();
            Container.BindInterfacesTo<PlayerHandler>().AsSingle();
        }

        private void BindSignals()
        {
            SignalBusInstaller.Install(Container);
        }
    }
}