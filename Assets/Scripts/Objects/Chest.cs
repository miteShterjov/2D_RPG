using EntityControl;
using EntityStats;
using Interface;
using UnityEngine;

namespace Objects
{
    public class Chest : MonoBehaviour, IDamageable
    {
        [Header("Chest config")]
        [SerializeField] private Vector2 knockbackForce = new Vector2(0f, 1.75f);
        
        private Animator Anim => GetComponentInChildren<Animator>();
        private Rigidbody2D Rb => GetComponent<Rigidbody2D>();
        private EntityVFX vfx => GetComponent<EntityVFX>();
        
        private static readonly int OpenChest = Animator.StringToHash("chestOpen");

        public bool TakeDamage(
            float damage, 
            float elementalDamage, 
            ElementType elementType, 
            Transform damageSource)
        {
            vfx?.PlayOnDamageFlashVFX();
            Anim?.SetBool(OpenChest, true);
            Rb.linearVelocity = knockbackForce;
            
            Rb.angularVelocity = Random.Range(-200f, 200f);
            
            return true;
            
            // for another day drop items logic here
        }
    }
}