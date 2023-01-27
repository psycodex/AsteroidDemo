using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Installers;
using Managers;
using Settings;
using Signals;
using UnityEngine;
using Utils;
using Zenject;

public class PlayerHandler : ITickable, IFixedTickable, ILateTickable
{
    [Inject] readonly Player _player;
    [Inject] readonly GameManager _gameManager;
    [Inject] readonly GameSettings _gameSettings;
    [Inject] private GameScriptableSettings _scriptableSettings;
    [Inject] private readonly Bullet.BulletsPool _bulletsPool;
    [Inject] private readonly CrescentBullet.CrescentBulletsPool _crescentBulletsPool;
    [Inject] private SignalBus _signalBus;

    private bool _isMovingLeft;
    private bool _isMovingRight;
    private bool _isMovingUp;
    private bool _isMovingDown;
    private bool _isFiring;
    private float _lastFireTime;
    private float _health;

    private bool _isShieldActive;
    private bool _isCrescentMoonBulletActive;

    private float _shieldActiveTime;
    private float _crescentMoonBulletsActiveTime;

    public bool IsShieldActive => _isShieldActive;

    public bool IsCrescentMoonBulletActive => _isCrescentMoonBulletActive;
    public void Tick()
    {
        if (_gameManager.CurrentState != Constants.GameStates.Playing)
        {
            return;
        }

        // var mouseRay = _gameSettings.MainCamera.ScreenPointToRay(Input.mousePosition);
        // var mousePos = new Vector2(mouseRay.origin.x, mouseRay.origin.y);
        // Vector2 direction = mousePos - _player.Position;
        // direction.Normalize();
        // _player.Rotation = Quaternion.FromToRotation(Vector3.up, direction);

        _isMovingLeft = Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A);
        _isMovingRight = Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D);
        _isMovingUp = Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W);
        _isMovingDown = Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S);
        _isFiring = Input.GetKey(KeyCode.Space) || Input.GetMouseButton(0);

        if (_isFiring && _scriptableSettings.Player.FirePerSecond != 0 && Time.realtimeSinceStartup - _lastFireTime >
            1 / _scriptableSettings.Player.FirePerSecond)
        {
            _lastFireTime = Time.realtimeSinceStartup;
            var position = _player.spawnPoint.transform.position;
            _player.StartCoroutine(BurstShoot(position));
        }

        if (_isShieldActive && Time.realtimeSinceStartup - _scriptableSettings.PowerUp.PlayerPowerUpLiveDuration >
            _shieldActiveTime)
        {
            DisableShield();
        }

        if (_isCrescentMoonBulletActive &&
            Time.realtimeSinceStartup - _scriptableSettings.PowerUp.PlayerPowerUpLiveDuration >
            _crescentMoonBulletsActiveTime)
        {
            DisableCrescentMoonBullets();
        }
    }

    IEnumerator BurstShoot(Vector3 position)
    {
        for (int i = 0; i < _scriptableSettings.Bullet.BurstMax; i++)
        {
            if (_isCrescentMoonBulletActive)
            {
                var bullet = _crescentBulletsPool.Add();
                var transform = bullet.transform;
                transform.position = position;
                transform.rotation = _player.spawnPoint.transform.rotation;
                bullet.Rigidbody2D.velocity =
                    (_player.Rigidbody2D.transform.up * _scriptableSettings.Bullet.BulletSpeed);
            }
            else
            {
                var bullet = _bulletsPool.Add();
                var transform = bullet.transform;
                transform.position = position;
                transform.rotation = _player.spawnPoint.transform.rotation;
                bullet.Rigidbody2D.velocity =
                    (_player.Rigidbody2D.transform.up * _scriptableSettings.Bullet.BulletSpeed);
            }

            yield return new WaitForSeconds(_scriptableSettings.Bullet.BurstIntervalMillSec);
        }
    }

    public void FixedTick()
    {
        if (_isMovingLeft)
        {
            _player.Rigidbody2D.AddTorque(_scriptableSettings.Player.AngularSpeed);
        }

        if (_isMovingRight)
        {
            _player.Rigidbody2D.AddTorque(-_scriptableSettings.Player.AngularSpeed);
        }

        if (_isMovingUp)
        {
            _player.Rigidbody2D.AddForce(
                _player.transform.up * _scriptableSettings.Player.MoveSpeed);
        }

        if (_isMovingDown)
        {
            _player.Rigidbody2D.AddForce(
                -_player.transform.up * _scriptableSettings.Player.MoveSpeed);
        }
    }

    public void TakeDamage(float size)
    {
        _health -= size;
        _signalBus.Fire(new UpdateHealthSignal
        {
            MaxHealth = _scriptableSettings.Player.Health,
            CurrentHealth = _health
        });
        if (_health > 0) return;

        // Die Game over
        _signalBus.Fire<GameOverSignal>();
        _player.gameObject.SetActive(false);
        var transform = _player.transform;
        var explosionObject = Object.Instantiate(_gameSettings.Views.ExplosionPrefab, transform.position,
            transform.rotation);
        Object.Destroy(explosionObject, 1.5f);
    }

    public void LateTick()
    {
        _player.CheckForTeleport(_gameSettings.MainCamera, _player.Rigidbody2D);
    }

    public void Reset()
    {
        _health = _scriptableSettings.Player.Health;
        _player.gameObject.SetActive(true);
        _player.Rigidbody2D.velocity = Vector2.zero;
        _player.transform.position = Vector2.zero;
        _player.Rotation = Quaternion.identity;
        _signalBus.Fire(new UpdateHealthSignal
        {
            CurrentHealth = _health,
            MaxHealth = _scriptableSettings.Player.Health
        });
    }

    public void OnPowerUp(Constants.PowerUpsType powerUpsType)
    {
        switch (powerUpsType)
        {
            case Constants.PowerUpsType.Shield:
                EnableShield();
                break;
            case Constants.PowerUpsType.CrescentMoon:
                EnableCrescentMoonBullets();
                break;
        }
    }

    private void EnableCrescentMoonBullets()
    {
        _isCrescentMoonBulletActive = true;
        _crescentMoonBulletsActiveTime = Time.realtimeSinceStartup;
    }

    private void DisableCrescentMoonBullets()
    {
        _isCrescentMoonBulletActive = false;
        _crescentMoonBulletsActiveTime = 0;
    }

    private void EnableShield()
    {
        _player.Collider2D.enabled = false;
        _player.Shield.SetActive(true);
        _isShieldActive = true;
        _shieldActiveTime = Time.realtimeSinceStartup;
    }

    private void DisableShield()
    {
        _player.Collider2D.enabled = true;
        _player.Shield.SetActive(false);
        _isShieldActive = false;
        _shieldActiveTime = 0;
    }
}