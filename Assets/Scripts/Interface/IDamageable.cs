using UnityEngine;

namespace Interface
{
    public interface IDamageable
    {
        public bool TakeDamage(float damage, Transform damageSource);
    }
}