using EntityControl;
using UnityEngine;

namespace EnemyControl
{
    public class EnemyHealthController : EntityHealthController
    {
        private EnemyController enemyController;

        protected override void Awake()
        {
            enemyController = GetComponent<EnemyController>();
        }
        
        public override void TakeDamage(float damage, Transform damageSource)
        {
            base.TakeDamage(damage, damageSource);
            
            if (isDead) return;
            
            if (damageSource.CompareTag("Player")) enemyController.TryEnterBattleState(damageSource);
        }
    }
}