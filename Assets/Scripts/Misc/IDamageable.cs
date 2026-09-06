using UnityEngine;

namespace Misc
{
    public interface IDamagable
    {
        public void TakeDamage(float damage, Transform damageSource);
    }
}