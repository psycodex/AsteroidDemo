using System;
using System.Collections;
using System.Collections.Generic;
using Installers;
using Settings;
using UnityEngine;
using Zenject;
using Zenject.SpaceFighter;

public class Asteroid : MonoBehaviour, IPoolable<IMemoryPool>, IDisposable
{
    [Inject] private GameSettings _gameSettings;
    [Inject] private GameScriptableSettings _scriptableSettings;
    private IMemoryPool _pool;

    public void OnSpawned(IMemoryPool p2)
    {
        _pool = p2;
        GetRandomVelocity();
    }

    public void OnDespawned()
    {
        _pool = null;
    }

    public void Dispose()
    {
    }

    private void Update()
    {
        CheckForTeleport();
    }

    private void GetRandomVelocity()
    {
        var minSpeed = _scriptableSettings.Asteroid.MinSpeed;
        var maxSpeed = _scriptableSettings.Asteroid.MaxSpeed;
        
    }

    private void CheckForTeleport()
    {
        //you get a world space coord and transfom it to viewport space.
        Vector3 pos = _gameSettings.MainCamera.WorldToViewportPoint(transform.position);

        //everything from here on is in viewport space where 0,0 is the bottom 
        //left of your screen and 1,1 the top right.
        if (pos.x < 0.0f)
        {
            pos = new Vector3(1.0f, pos.y, pos.z);
        }
        else if (pos.x >= 1.0f)
        {
            pos = new Vector3(0.0f, pos.y, pos.z);
        }

        if (pos.y < 0.0f)
        {
            pos = new Vector3(pos.x, 1.0f, pos.z);
        }
        else if (pos.y >= 1.0f)
        {
            pos = new Vector3(pos.x, 0.0f, pos.z);
        }

        //and here it gets transformed back to world space.
        transform.position = _gameSettings.MainCamera.ViewportToWorldPoint(pos);
    }

    public class AsteroidPool
    {
        // private readonly List<Bullet> _asteroids = new List<Asteroid>();
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