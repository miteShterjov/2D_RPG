using UnityEngine;
namespace Misc
{
    public class ParallaxBackground : MonoBehaviour
    {
        private Camera _mainCamera;
        private float _lastCameraPositionX;
        private float _cameraHalfWidth;

        [SerializeField] private ParallaxLayer[] backgroundLayers;

        private void Awake()
        {
            _mainCamera = Camera.main;
            if (_mainCamera == null) return;

            _cameraHalfWidth = _mainCamera.orthographicSize * _mainCamera.aspect;
            _lastCameraPositionX = _mainCamera.transform.position.x;
            InitializeLayers();
        }

        private void FixedUpdate()
        {
            if (_mainCamera == null) return;

            float currentCameraPositionX = _mainCamera.transform.position.x;
            float distanceToMove = currentCameraPositionX - _lastCameraPositionX;
            _lastCameraPositionX = currentCameraPositionX;

            float cameraLeftEdge = currentCameraPositionX - _cameraHalfWidth;
            float cameraRightEdge = currentCameraPositionX + _cameraHalfWidth;

            foreach (ParallaxLayer layer in backgroundLayers)
            {
                layer.Move(distanceToMove);
                layer.LoopBackground(cameraLeftEdge, cameraRightEdge);
            }
        }

        private void InitializeLayers()
        {
            foreach (ParallaxLayer layer in backgroundLayers)
            {
                if (layer != null) layer.CalculateImageWidth();
            }
        }
    }
}
