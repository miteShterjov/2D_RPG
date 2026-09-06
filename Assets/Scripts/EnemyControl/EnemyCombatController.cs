using EntityControl;
using UnityEngine;

namespace EnemyControl
{
    public class EnemyCombatController : EntityCombatController
    {
        private EnemyController enemyController;

        protected override void Awake()
        {
            base.Awake();
            enemyController = GetComponent<EnemyController>();
        }

        public override void PreformAttackEffect()
        {
            if (!enemyController.IsAttackStateActive) return;
            base.PreformAttackEffect();
        }
    }
}