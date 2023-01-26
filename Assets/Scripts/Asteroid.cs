using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Installers;
using Managers;
using Settings;
using UnityEngine;
using Utils;
using Zenject;
using Zenject.SpaceFighter;
using Random = UnityEngine.Random;

public class Asteroid : MonoBehaviour, IPoolable<IMemoryPool>, IDisposable
{
    [Inject] private GameSettings _gameSettings;
    [Inject] private GameScriptableSettings _scriptableSettings;
    [Inject] private GameManager _gameManager;
    [Inject] private AsteroidsPool _asteroidsPool;
    private IMemoryPool _pool;
    private int _scale;

    public Rigidbody2D Rigidbody2D { get; private set; }

    public Collider2D Collider2D { get; private set; }

    public SpriteRenderer ARenderer { get; private set; }
    // public Transform Transform { get; private set; }

    public void OnSpawned(IMemoryPool p2)
    {
        _pool = p2;
        Rigidbody2D = GetComponent<Rigidbody2D>();
        Collider2D = GetComponent<Collider2D>();
        ARenderer = GetComponent<SpriteRenderer>();
    }

    public void OnDespawned()
    {
        transform.localScale = new Vector2(1, 1);
        Rigidbody2D.mass = 1;
        Rigidbody2D.velocity = Vector2.zero;
        _pool = null;
    }

    public void Dispose()
    {
        _pool?.Despawn(this);
    }

    private void Update()
    {
        if (_gameManager.CurrentState != Constants.GameStates.Playing)
        {
            return;
        }

        int level = _gameManager.Level;
        if (Rigidbody2D.velocity.magnitude < _scriptableSettings.Level.Levels[level].MinSpeed)
        {
            var dir = Rigidbody2D.velocity.normalized;
            Rigidbody2D.velocity = dir * _scriptableSettings.Level.Levels[level].MinSpeed;
        }
        else if (Rigidbody2D.velocity.magnitude > _scriptableSettings.Level.Levels[level].MaxSpeed)
        {
            var dir = Rigidbody2D.velocity.normalized;
            Rigidbody2D.velocity = dir * _scriptableSettings.Level.Levels[level].MaxSpeed;
        }
    }

    private void LateUpdate()
    {
        this.CheckForTeleport(_gameSettings.MainCamera, Rigidbody2D);
    }

    public void DestroyAsteroid(Vector2 bulletVelocity, Vector3 position)
    {
        var velocity = Rigidbody2D.velocity;
        _asteroidsPool.Remove(this);
        var scale = _scale + 1;
        if (scale > 2)
        {
            return;
        }

        Vector3 leftAsteroidDirection = Quaternion.Euler(0, 0, 45) * (bulletVelocity + Rigidbody2D.velocity);
        var leftAsteroid = _asteroidsPool.Add();
        leftAsteroid.SetMeta(position, leftAsteroidDirection, scale);
        Vector3 rightAsteroidDirection = Quaternion.Euler(0, 0, -45) * (bulletVelocity + Rigidbody2D.velocity);
        var rightAsteroid = _asteroidsPool.Add();
        rightAsteroid.SetMeta(position, rightAsteroidDirection, scale);
    }

    public void SetMeta(Vector3 position, Vector3 velocity, int scale)
    {
        _scale = scale;
        transform.position = position;
        var size = Mathf.Pow(2, -scale);
        var mass = size * _scriptableSettings.Asteroid.ScaleMassFactor;
        transform.localScale = new Vector2(size, size);
        Rigidbody2D.mass = mass;
        Rigidbody2D.velocity = velocity;
    }

    public class AsteroidsPool
    {
        private readonly List<Asteroid> _asteroids = new List<Asteroid>();

        private readonly Factory _factory;

        public AsteroidsPool(Factory factory)
        {
            _factory = factory;
        }

        public Asteroid Add()
        {
            var asteroid = _factory.Create();
            _asteroids.Add(asteroid);
            return asteroid;
        }

        public void Remove(Asteroid asteroid)
        {
            _asteroids.Remove(asteroid);
            asteroid.Dispose();
        }

        public void RemoveAll()
        {
            foreach (var asteroid in _asteroids.ToList())
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