using Settings;
using UnityEngine;
using Zenject;

namespace Managers
{
    public class WorldManager : IInitializable
    {
        [Inject] private GameSettings _settings;
        private float _width;
        private float _height;

        public float Width => _width;

        public float Height => _height;

        public void Initialize()
        {
            // Debug.Log("Initialised");
            var camera = _settings.MainCamera;
            _width = camera.aspect * camera.orthographicSize;
            _height = _settings.MainCamera.orthographicSize;
        }
    }
}