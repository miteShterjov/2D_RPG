using UnityEngine;

namespace Interface
{
    public interface IDamageable
    {
        public void TakeDamage(float damage, Transform damageSource);
    }
}