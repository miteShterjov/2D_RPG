using System;
using UnityEngine;

namespace EntityControl
{
    public class EntityMoveController : MonoBehaviour
    {
        public Rigidbody2D rb;
        public int FacingDir => facingDir;
        
        [Header("Locomotion")]
        [SerializeField] public float moveSpeed = 10f;
        [SerializeField] private bool isFacingRight = true;
        [SerializeField] private int facingDir = 1;

        protected virtual void Awake()
        {
            rb = GetComponent<Rigidbody2D>();
        }
        
        public void SetVelocity(float x, float y)
        {
            rb.linearVelocity = new Vector2(x, y);
            HandleFlipEntitySprite(x);
        }
        
        public void SetVelocity(Vector2 velocity)
        {
            rb.linearVelocity = velocity;
            HandleFlipEntitySprite(velocity.x);
        }
        
        public void FlipEntitySprite()
        {
            transform.localScale = new Vector3(isFacingRight ? -1 : 1, 1, 1);
            isFacingRight = !isFacingRight;
            facingDir = isFacingRight ? 1 : -1;
        }
    
        private void HandleFlipEntitySprite(float xVelocity)
        {
            if (xVelocity > 0 && !isFacingRight || xVelocity < 0 && isFacingRight) FlipEntitySprite();
        }
    }
}