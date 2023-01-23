using System;
using System.Collections;
using System.Collections.Generic;
using Installers;
using Managers;
using Settings;
using UnityEngine;
using Zenject;
using Zenject.SpaceFighter;
using Random = UnityEngine.Random;

public class Asteroid : MonoBehaviour, IPoolable<IMemoryPool>, IDisposable
{
    [Inject] private GameSettings _gameSettings;
    [Inject] private GameScriptableSettings _scriptableSettings;
    [Inject] private WorldManager _worldManager;
    private IMemoryPool _pool;

    public Rigidbody2D Rigidbody2D { get; private set; }

    public Collider2D Collider2D { get; private set; }
    // public Transform Transform { get; private set; }

    public void OnSpawned(IMemoryPool p2)
    {
        _pool = p2;
        GetRandomPositionAndVelocity();
        Rigidbody2D = GetComponent<Rigidbody2D>();
        Collider2D = GetComponent<Collider2D>();
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
        // CheckForTeleport();
    }

    private void GetRandomPositionAndVelocity()
    {
        var minSpeed = _scriptableSettings.Asteroid.MinSpeed;
        var maxSpeed = _scriptableSettings.Asteroid.MaxSpeed;

        var x = Random.Range(-_worldManager.Width, _worldManager.Width);
        var y = Random.Range(-_worldManager.Height, _worldManager.Height);
        transform.position = new Vector3(x, y, 0);
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

    public class AsteroidsPool
    {
        private readonly List<Asteroid> _asteroids = new List<Asteroid>();

        private readonly Factory _factory;
        // [Inject] private GameScriptableSettings _scriptableSettings;

        public AsteroidsPool(Factory factory)
        {
            _factory = factory;
        }

        public Asteroid Add()
        {
            var asteroid = _factory.Create();
            return asteroid;
        }

        public void Remove(Asteroid asteroid)
        {
            _asteroids.Remove(asteroid);
            asteroid.Dispose();
        }

        public void RemoveAll()
        {
            foreach (var asteroid in _asteroids)
            {
                _asteroids.Remove(asteroid);
                asteroid.Dispose();
            }
        }
    }

    public class Factory : PlaceholderFactory<Asteroid>
    {
    }
}