using Installers;
using Managers;
using Settings;
using UnityEngine;
using Zenject;

public class PlayerHandler : ITickable, IFixedTickable
{
    [Inject] readonly Player _player;
    [Inject] readonly GameManager _gameManager;
    [Inject] readonly GameSettings _gameSettings;
    [Inject] private GameScriptableSettings _scriptableSettings;
    [Inject] private readonly Bullet.BulletsPool _bulletsPool;

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

        if (_isFiring && Time.realtimeSinceStartup - _lastFireTime > _scriptableSettings.Player.FirePerSecond)
        {
            _lastFireTime = Time.realtimeSinceStartup;
            var bullet = _bulletsPool.Add(_player.spawnPoint.transform.position);
            bullet.transform.position = _player.spawnPoint.transform.position;
            bullet.transform.rotation = _player.spawnPoint.transform.rotation;
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
}