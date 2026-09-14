using UnityEngine;

namespace Misc
{
    public class ParallaxLayer : MonoBehaviour
    {
        [SerializeField] private Transform background;
        [SerializeField] private float parallaxMultiplier;
        [SerializeField] private float imageWidthOffset = 10;

        private float _imageFullWidth;
        private float _imageHalfWidth;

        public void CalculateImageWidth()
        {
            if (background == null) return;

            SpriteRenderer spriteRenderer = background.GetComponent<SpriteRenderer>();
            if (spriteRenderer == null) return;

            _imageFullWidth = spriteRenderer.bounds.size.x;
            _imageHalfWidth = _imageFullWidth / 2;
        }

        public void Move(float distanceToMove)
        {
            if (background == null) return;
            background.position += Vector3.right * (distanceToMove * parallaxMultiplier);
        }

        public void LoopBackground(float cameraLefteEdge, float cameraRightEdge)
        {
            if (background == null || _imageFullWidth <= 0f) return;

            float imageRightEdge = (background.position.x + _imageHalfWidth) - imageWidthOffset;
            float imageLeftEdge = (background.position.x - _imageHalfWidth) + imageWidthOffset;

            if (imageRightEdge < cameraLefteEdge)
                background.position += Vector3.right * _imageFullWidth;
            else if (imageLeftEdge > cameraRightEdge)
                background.position += Vector3.right * -_imageFullWidth;
        }
    }
}