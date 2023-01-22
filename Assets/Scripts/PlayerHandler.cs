using System.Collections;
using System.Collections.Generic;
using Settings;
using UnityEngine;
using Zenject;

public class PlayerHandler : ITickable
{
    [Inject] readonly Player _player;
    [Inject] readonly GameSettings _gameSettings;

    public void Tick()
    {
    }
}