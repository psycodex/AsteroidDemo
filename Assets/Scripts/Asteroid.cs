using System;
using System.Collections;
using System.Collections.Generic;
using Installers;
using UnityEngine;
using Zenject;
using Zenject.SpaceFighter;

public class Asteroid : MonoBehaviour, IPoolable< IMemoryPool>, IDisposable
{
    public void OnDespawned()
    {
    }

    public void OnSpawned( IMemoryPool p2)
    {
    }

    public void Dispose()
    {
    }

    public class AsteroidPool
    {
        // private readonly List<Bullet> _asteroids = new List<Bullet>();
        private readonly Factory _factory;
        [Inject] private GameScriptableSettings _scriptableSettings;

        public AsteroidPool(Factory factory)
        {
            _factory = factory;
        }

        public Asteroid Add()
        {
            var asteroid = _factory.Create();
            return asteroid;
        }

        public void Remove()
        {
            
        }
    }

    public class Factory : PlaceholderFactory<Asteroid>
    {
    }
}