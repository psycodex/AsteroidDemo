using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Installers;
using UnityEngine;
using Zenject;

public class PowerUp : MonoBehaviour, IPoolable<Constants.PowerUpsType, IMemoryPool>, IDisposable
{
    [Inject] private GameScriptableSettings _scriptableSettings;
    [Inject] private PlayerHandler _playerHandler;

    private IMemoryPool _pool;
    private float _startTime;
    [SerializeField] private Constants.PowerUpsType powerUpsType;
    [SerializeField] private GameObject[] powerUpsGameObject;
    public Rigidbody2D Rigidbody2D { get; private set; }
    public Collider2D Collider2D { get; private set; }

    public void OnDespawned()
    {
        _pool = null;

        foreach (var o in powerUpsGameObject)
        {
            o.SetActive(false);
        }
    }

    public void OnSpawned(Constants.PowerUpsType type, IMemoryPool p1)
    {
        powerUpsType = type;
        if (powerUpsType == Constants.PowerUpsType.CrescentMoon)
        {
            powerUpsGameObject[0].SetActive(true);
            Rigidbody2D = powerUpsGameObject[0].GetComponent<Rigidbody2D>();
            Collider2D = powerUpsGameObject[0].GetComponent<Collider2D>();
        }
        else if (powerUpsType == Constants.PowerUpsType.Shield)
        {
            powerUpsGameObject[1].SetActive(true);
            Rigidbody2D = powerUpsGameObject[1].GetComponent<Rigidbody2D>();
            Collider2D = powerUpsGameObject[1].GetComponent<Collider2D>();
        }
        else
        {
            throw new Exception("Invalid powerup type");
        }

        _startTime = Time.realtimeSinceStartup;
        _pool = p1;
        StartCoroutine(DelayDie());
    }

    private IEnumerator DelayDie()
    {
        while (true)
        {
            if (Time.realtimeSinceStartup - _startTime > _scriptableSettings.PowerUp.SpawnPowerUpLiveDuration)
            {
                break;
            }

            yield return new WaitForEndOfFrame();
        }

        Dispose();
    }

    public void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag(Constants.TagPlayer))
        {
            _playerHandler.OnPowerUp(powerUpsType);
        }

        Dispose();
    }

    public void Dispose()
    {
        _pool?.Despawn(this);
    }

    public class PowerUpPool
    {
        private readonly List<PowerUp> _powerUps = new List<PowerUp>();
        private readonly Factory _factory;

        public PowerUpPool(Factory factory)
        {
            _factory = factory;
        }

        public PowerUp Add(Constants.PowerUpsType type)
        {
            var powerUp = _factory.Create(type);
            _powerUps.Add(powerUp);
            return powerUp;
        }

        public void Remove(PowerUp powerUp)
        {
            _powerUps.Remove(powerUp);
            powerUp.Dispose();
        }

        public void RemoveAll()
        {
            foreach (var bullet in _powerUps.ToList())
            {
                _powerUps.Remove(bullet);
                bullet.Dispose();
            }
        }
    }

    public class Factory : PlaceholderFactory<Constants.PowerUpsType, PowerUp>
    {
    }
}