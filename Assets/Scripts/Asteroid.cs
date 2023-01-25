using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
    [Inject] private GameManager _gameManager;
    private IMemoryPool _pool;

    public Rigidbody2D Rigidbody2D { get; private set; }

    public Collider2D Collider2D { get; private set; }

    public SpriteRenderer SpriteRenderer { get; private set; }
    // public Transform Transform { get; private set; }

    public void OnSpawned(IMemoryPool p2)
    {
        _pool = p2;
        Rigidbody2D = GetComponent<Rigidbody2D>();
        Collider2D = GetComponent<Collider2D>();
        SpriteRenderer = GetComponent<SpriteRenderer>();
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

        CheckForTeleport();
    }

    private void CheckForTeleport()
    {
        Vector3 pos = _gameSettings.MainCamera.WorldToViewportPoint(transform.position);
        if (pos.x < 0.0f && IsMovingInDirection(Vector3.left))
        {
            pos = new Vector3(1.0f, pos.y, pos.z);
        }
        else if (pos.x >= 1.0f && IsMovingInDirection(Vector3.right))
        {
            pos = new Vector3(0.0f, pos.y, pos.z);
        }

        if (pos.y < 0.0f && IsMovingInDirection(Vector3.down))
        {
            pos = new Vector3(pos.x, 1.0f, pos.z);
        }
        else if (pos.y >= 1.0f && IsMovingInDirection(Vector3.up))
        {
            pos = new Vector3(pos.x, 0.0f, pos.z);
        }

        transform.position = _gameSettings.MainCamera.ViewportToWorldPoint(pos);
    }

    bool IsMovingInDirection(Vector3 dir)
    {
        return Vector3.Dot(dir, Rigidbody2D.velocity) > 0;
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