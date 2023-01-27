using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Installers;
using Managers;
using Settings;
using Signals;
using UnityEngine;
using Utils;
using Zenject;

public class Asteroid : MonoBehaviour, IPoolable<IMemoryPool>, IDisposable
{
    [Inject] private GameSettings _gameSettings;
    [Inject] private GameScriptableSettings _scriptableSettings;
    [Inject] private GameManager _gameManager;
    [Inject] private AsteroidsPool _asteroidsPool;
    [Inject] private SignalBus _signalBus;
    [Inject] private Play _play;
    private IMemoryPool _pool;
    private int _scale;
    private float _size;
    private bool isDestroyed;

    public Rigidbody2D Rigidbody2D { get; private set; }

    public Collider2D Collider2D { get; private set; }

    public SpriteRenderer ARenderer { get; private set; }
    // public Transform Transform { get; private set; }

    public float Size => _size;

    public void OnSpawned(IMemoryPool p2)
    {
        isDestroyed = false;
        _pool = p2;
        Rigidbody2D = GetComponent<Rigidbody2D>();
        Collider2D = GetComponent<Collider2D>();
        ARenderer = GetComponent<SpriteRenderer>();
        _size = _scriptableSettings.Asteroid.DefaultScale;
        transform.localScale = new Vector2(_scriptableSettings.Asteroid.DefaultScale,
            _scriptableSettings.Asteroid.DefaultScale);
    }

    public void OnDespawned()
    {
        isDestroyed = true;
        _size = _scriptableSettings.Asteroid.DefaultScale;
        transform.localScale = new Vector2(_scriptableSettings.Asteroid.DefaultScale,
            _scriptableSettings.Asteroid.DefaultScale);
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

        // int level = _gameManager.Level;
        if (Rigidbody2D.velocity.magnitude < _scriptableSettings.Asteroid.MinSpeed)
        {
            var dir = Rigidbody2D.velocity.normalized;
            Rigidbody2D.velocity = dir * _scriptableSettings.Asteroid.MinSpeed;
        }
        else if (Rigidbody2D.velocity.magnitude > _scriptableSettings.Asteroid.MaxSpeed)
        {
            var dir = Rigidbody2D.velocity.normalized;
            Rigidbody2D.velocity = dir * _scriptableSettings.Asteroid.MaxSpeed;
        }
    }

    private void LateUpdate()
    {
        this.CheckForTeleport(_gameSettings.MainCamera, Rigidbody2D);
    }

    public void DestroyAsteroid(Vector2 bulletVelocity, Vector3 position)
    {
        if (isDestroyed)
        {
            return;
        }

        isDestroyed = true;
        _signalBus.Fire<IncrementScoreSignal>();
        _play.StartCoroutine(SpawnSubAsteroid(bulletVelocity, position));
    }

    private IEnumerator SpawnSubAsteroid(Vector2 bulletVelocity, Vector3 position)
    {
        _asteroidsPool.Remove(this);
        yield return new WaitForEndOfFrame();
        var scale = _scale + 1;
        if (scale > _scriptableSettings.Asteroid.MaxScale)
        {
            yield break;
        }

        Vector3 leftAsteroidDirection = Quaternion.Euler(0, 0, 45) * (bulletVelocity + Rigidbody2D.velocity);
        var leftAsteroid = _asteroidsPool.Add();
        leftAsteroid.SetMeta(position, leftAsteroidDirection, scale);

        yield return new WaitForEndOfFrame();
        Vector3 rightAsteroidDirection = Quaternion.Euler(0, 0, -45) * (bulletVelocity + Rigidbody2D.velocity);
        var rightAsteroid = _asteroidsPool.Add();
        rightAsteroid.SetMeta(position, rightAsteroidDirection, scale);
    }

    public void SetMeta(Vector3 position, Vector3 velocity, int scale)
    {
        _scale = scale;
        transform.position = position;
        _size = Mathf.Pow(2, -scale);
        var mass = _size * _scriptableSettings.Asteroid.ScaleMassFactor;
        transform.localScale = new Vector2(_size, _size);
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