using UnityEngine;

namespace Misc
{
    public interface IDamageable
    {
        public void TakeDamage(float damage, Transform damageSource);
    }
}