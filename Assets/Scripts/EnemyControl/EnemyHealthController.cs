using EntityControl;
using EntityStats;
using UnityEngine;

namespace EnemyControl
{
    public class EnemyHealthController : EntityHealthController
    {
        private EnemyController enemyController;

        protected override void Awake()
        {
            base.Awake();
            enemyController = GetComponent<EnemyController>();
        }
        
        public override bool TakeDamage(float damage, float elementalDamage, ElementType elementType,
            Transform damageSource)
        {
            if (!base.TakeDamage(damage, 0, ElementType.None, damageSource)) return false;
            
            if (damageSource.CompareTag("Player")) enemyController.TryEnterBattleState(damageSource);
            
            return true;
        }
    }
}