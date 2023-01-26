using Managers;
using Settings;
using Signals;
using UI;
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
            Container.BindInterfacesAndSelfTo<WorldManager>().AsSingle();
            Container.BindInterfacesAndSelfTo<PlayerHandler>().AsSingle();
            Container.BindInterfacesAndSelfTo<AsteroidManager>().AsSingle();
            Container.BindInterfacesAndSelfTo<GameManager>().AsSingle();
            Container.Bind<Bullet.BulletsPool>().AsSingle();
            Container.Bind<Asteroid.AsteroidsPool>().AsSingle();
            Container.BindInterfacesAndSelfTo<Play>().FromComponentInHierarchy(settings.Views.Play).AsSingle();
            Container.BindInterfacesAndSelfTo<HomeView>().FromComponentInHierarchy(settings.Views.HomeView).AsSingle();
            Container.BindInterfacesAndSelfTo<GamePlayView>().FromComponentInHierarchy(settings.Views.GamePlayView)
                .AsSingle();
            Container.BindInterfacesAndSelfTo<GameOverView>().FromComponentInHierarchy(settings.Views.GameOverView)
                .AsSingle();
            Container.BindInterfacesAndSelfTo<Player>().FromComponentInHierarchy(settings.Views.Player).AsSingle();
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

            Container.DeclareSignal<GameStartSignal>();
            Container.DeclareSignal<GameOverSignal>();
            Container.DeclareSignal<IncrementScoreSignal>();
        }
    }

    internal class BulletFacadePool : MonoPoolableMemoryPool<IMemoryPool, Bullet>
    {
    }

    internal class AsteroidFacadePool : MonoPoolableMemoryPool<IMemoryPool, Asteroid>
    {
    }
}