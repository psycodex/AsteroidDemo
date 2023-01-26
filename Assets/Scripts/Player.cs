using System;
using System.Runtime.InteropServices;
using UnityEngine;
using Zenject;

public class Player : MonoBehaviour
{
    [Inject] private PlayerHandler _playerHandler;
    public GameObject spawnPoint;
    public Rigidbody2D Rigidbody2D { get; private set; }

    public Collider2D Collider2D { get; private set; }
    // public Transform Transform { get; private set; }

    public Vector2 Position => Rigidbody2D.position;

    public Quaternion Rotation
    {
        get => transform.rotation;
        set => transform.rotation = value;
    }

    private void Start()
    {
        Rigidbody2D = GetComponent<Rigidbody2D>();
        Collider2D = GetComponent<Collider2D>();
        // Transform = GetComponent<Transform>();
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        if (col.collider.CompareTag(Constants.TagAsteroid))
        {
            _playerHandler.TakeDamage();
        }
    }
}