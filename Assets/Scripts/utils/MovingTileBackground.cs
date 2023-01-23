using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingTileBackground : MonoBehaviour
{
    [SerializeField] float _speed;

    Vector2 _offset;
    SpriteRenderer _renderer;

    void Awake()
    {
        _renderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        _offset.y += _speed * Time.deltaTime;
        // _renderer.material.SetTextureOffset("_MainTex", new Vector2(0, -2.0f));
        _renderer.material.mainTextureOffset = _offset;
    }
}