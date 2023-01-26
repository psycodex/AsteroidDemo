using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpCrescentMoon : MonoBehaviour
{
    [SerializeField] private PowerUp powerUp;

    private void OnTriggerEnter2D(Collider2D col)
    {
        powerUp.OnTriggerEnter2D(col);
    }
}