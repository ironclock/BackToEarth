using UnityEngine;

namespace Platformer.View
{
    /// <summary>
    /// Used to move a transform relative to the main camera position with a scale factor applied.
    /// This is used to implement parallax scrolling effects on different branches of gameobjects.
    /// </summary>
    public class ParallaxLayer : MonoBehaviour
    {
        /// <summary>
        /// Movement of the layer is scaled by this value.
        /// </summary>
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