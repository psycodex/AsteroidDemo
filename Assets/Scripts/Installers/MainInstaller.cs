using Managers;
using Settings;
using UnityEngine;
using Zenject;

namespace Installers
{
    public class MainInstaller : MonoInstaller<MainInstaller>
    {
        [SerializeField] private GameSettings settings;

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
            Container.Bind<Bullet.BulletsPool>().AsSingle();
            Container.BindInterfacesAndSelfTo<Player>().FromComponentInHierarchy(settings.Views.Player).AsSingle();
            Container.BindInterfacesTo<PlayerHandler>().AsSingle();
            Container.BindInterfacesAndSelfTo<AsteroidManager>().AsSingle();
            Container.BindInterfacesAndSelfTo<GameManager>().AsSingle();
            Container.BindFactory<Bullet, Bullet.Factory>()
                .FromPoolableMemoryPool<Bullet, BulletFacadePool>(
                    poolBinder => poolBinder
                        .WithInitialSize(5)
                        .FromComponentInNewPrefab(settings.Views.BulletPrefab)
                        .UnderTransform(settings.Views.Play.transform));
            Container.BindFactory<Asteroid, Asteroid.Factory>()
                .FromPoolableMemoryPool<Asteroid, AsteroidFacadePool>(
                    poolBinder => poolBinder
                        .WithInitialSize(5)
                        .FromComponentInNewPrefab(settings.Views.AsteroidPrefab)
                        .UnderTransform(settings.Views.Play.transform));
        }

        private void BindSignals()
        {
            SignalBusInstaller.Install(Container);
        }
    }

    internal class BulletFacadePool : MonoPoolableMemoryPool<IMemoryPool, Bullet>
    {
    }

    internal class AsteroidFacadePool : MonoPoolableMemoryPool<IMemoryPool, Asteroid>
    {
    }
}