using System;
using UnityEngine;

namespace EntityControl
{
    public class EntityMoveController : MonoBehaviour
    {
        public event Action<int> OnFlip;
        
        public Rigidbody2D rb;
        public int FacingDir => facingDir;
        public bool IsKnockedBack { get; set; }

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
            if (IsKnockedBack) return;
            
            rb.linearVelocity = new Vector2(x, y);
            HandleFlipEntitySprite(x);
        }
        
        public void SetVelocity(Vector2 velocity)
        {
            if (IsKnockedBack) return;
            
            rb.linearVelocity = velocity;
            HandleFlipEntitySprite(velocity.x);
        }
        
        public void FlipEntitySprite()
        {
            Vector3 scale = transform.localScale;
            transform.localScale = new Vector3(Mathf.Abs(scale.x) * (isFacingRight ? -1 : 1), scale.y, scale.z);
            isFacingRight = !isFacingRight;
            facingDir = isFacingRight ? 1 : -1;
            
            OnFlip?.Invoke(facingDir);
        }

        public void FlipEntitySprite(int facingSide)
        {
            facingSide = facingSide < 0 ? -1 : 1;
            Vector3 scale = transform.localScale;
            transform.localScale = new Vector3(Mathf.Abs(scale.x) * facingSide, scale.y, scale.z);
            isFacingRight = facingSide > 0;
            facingDir = facingSide;
            
            OnFlip?.Invoke(facingDir);
        }
        
        private void HandleFlipEntitySprite(float xVelocity)
        {
            if (xVelocity > 0 && !isFacingRight || xVelocity < 0 && isFacingRight) FlipEntitySprite();
        }
    }
}