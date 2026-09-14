using EntityControl;
using Interface;
using UnityEngine;

namespace Misc
{
    public class Chest : MonoBehaviour, IDamageable
    {
        [Header("Chest config")]
        [SerializeField] private Vector2 knockbackForce = new Vector2(0f, 1.75f);
        
        private Animator Anim => GetComponentInChildren<Animator>();
        private Rigidbody2D Rb => GetComponent<Rigidbody2D>();
        private EntityVFX vfx => GetComponent<EntityVFX>();
        
        private static readonly int OpenChest = Animator.StringToHash("chestOpen");

        public void TakeDamage(float damage, Transform damageSource)
        {
            vfx?.PlayOnDamageFlashVFX();
            Anim?.SetBool(OpenChest, true);
            Rb.linearVelocity = knockbackForce;
            
            Rb.angularVelocity = Random.Range(-200f, 200f);
            
            // for another day drop items logic here
        }
    }
}