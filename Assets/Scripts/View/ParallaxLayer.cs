using UnityEngine;

namespace Platformer.View
{
    public class ParallaxLayer : MonoBehaviour
    {
        public Vector3 movementScale = Vector3.one;
        public float initialVerticalOffset;
        public float initialHorizontalOffset;


        Transform _camera;

        void Awake()
        {
            _camera = Camera.main.transform;
        }

        void Update()
        {
            Vector3 _cameraPos = new Vector3(_camera.position.x + initialHorizontalOffset, _camera.position.y + initialVerticalOffset, 0);
            transform.position = Vector3.Scale(_cameraPos, movementScale);
        }

    }
}