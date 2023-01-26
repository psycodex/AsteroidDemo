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
    [Inject] private SignalBus _signalBus;

    private bool _isMovingLeft;
    private bool _isMovingRight;
    private bool _isMovingUp;
    private bool _isMovingDown;
    private bool _isFiring;
    private float _lastFireTime;


    public void Tick()
    {
        if (_gameManager.CurrentState != Constants.GameStates.Playing)
        {
            return;
        }

        var mouseRay = _gameSettings.MainCamera.ScreenPointToRay(Input.mousePosition);
        var mousePos = new Vector2(mouseRay.origin.x, mouseRay.origin.y);
        Vector2 direction = mousePos - _player.Position;
        direction.Normalize();
        _player.Rotation = Quaternion.FromToRotation(Vector3.up, direction);

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
            var bullet = _bulletsPool.Add(position);
            var transform = bullet.transform;
            transform.position = position;
            transform.rotation = _player.spawnPoint.transform.rotation;
            bullet.Rigidbody2D.velocity =
                (_player.Rigidbody2D.transform.up * _scriptableSettings.Bullet.BulletSpeed);
        }
    }

    public void FixedTick()
    {
        if (_isMovingLeft)
        {
            _player.Rigidbody2D.AddForce(
                Vector3.left * _scriptableSettings.Player.MoveSpeed);
        }

        if (_isMovingRight)
        {
            _player.Rigidbody2D.AddForce(
                Vector3.right * _scriptableSettings.Player.MoveSpeed);
        }

        if (_isMovingUp)
        {
            _player.Rigidbody2D.AddForce(
                Vector3.up * _scriptableSettings.Player.MoveSpeed);
        }

        if (_isMovingDown)
        {
            _player.Rigidbody2D.AddForce(
                Vector3.down * _scriptableSettings.Player.MoveSpeed);
        }
    }

    public void TakeDamage()
    {
        _signalBus.Fire<GameOverSignal>();
    }

    public void LateTick()
    {
        _player.CheckForTeleport(_gameSettings.MainCamera, _player.Rigidbody2D);
    }

    public void Reset()
    {
        _player.Rigidbody2D.velocity = Vector2.zero;
        _player.transform.position = Vector2.zero;
        _player.Rotation = Quaternion.identity;
    }
}