using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingTileBackground : MonoBehaviour
{
    [SerializeField] private Renderer _renderer;
    [SerializeField] float _speed;

    Vector2 _offset;

    void Update()
    {
        _offset.y += _speed * Time.deltaTime;
        _renderer.material.mainTextureOffset = _offset;
    }
}