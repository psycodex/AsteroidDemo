using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Installers;
using UnityEngine;
using Zenject;

public class Bullet : MonoBehaviour, IPoolable<IMemoryPool>, IDisposable
{
    private IMemoryPool _pool;
    private float _startTime;

    public Rigidbody2D Rigidbody2D { get; private set; }
    public Collider2D Collider2D { get; private set; }

    [Inject] private GameScriptableSettings _scriptableSettings;

    public void OnDespawned()
    {
        _pool = null;
    }

    public void OnSpawned(IMemoryPool p1)
    {
        _startTime = Time.realtimeSinceStartup;
        // transform.position = spawnPosition;
        _pool = p1;
        Rigidbody2D = GetComponent<Rigidbody2D>();
        Collider2D = GetComponent<Collider2D>();
        StartCoroutine(HandleBulletLifeTime());
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag(Constants.TagAsteroid))
        {
            var asteroid = col.GetComponent<Asteroid>();
            asteroid.DestroyAsteroid(Rigidbody2D.velocity, transform.position);
        }

        Dispose();
    }

    public void Dispose()
    {
        _pool?.Despawn(this);
    }

    IEnumerator HandleBulletLifeTime()
    {
        while (true)
        {
            if (Time.realtimeSinceStartup - _startTime < _scriptableSettings.Bullet.BulletLifeTime)
            {
                yield return new WaitForSeconds(0.1f);
            }
            else
            {
                break;
            }
        }

        _pool.Despawn(this);
    }

    public class BulletsPool
    {
        private readonly List<Bullet> _bullets = new List<Bullet>();
        private readonly Factory _factory;

        public BulletsPool(Factory factory)
        {
            _factory = factory;
        }

        public Bullet Add()
        {
            var bullet = _factory.Create();
            _bullets.Add(bullet);
            return bullet;
        }

        public void Remove(Bullet bullet)
        {
            _bullets.Remove(bullet);
            bullet.Dispose();
        }

        public void RemoveAll()
        {
            foreach (var bullet in _bullets.ToList())
            {
                _bullets.Remove(bullet);
                bullet.Dispose();
            }
        }
    }

    public class Factory : PlaceholderFactory<Bullet>
    {
    }
}