using EntityControl;
using UnityEngine;

namespace EnemyControl
{
    public class EnemyAnimTriggersHandler : EntityAnimTriggersHandler
    {
        private EnemyController enemyController;
        private Enemy_VFX enemyVFX;

        protected override void Awake()
        {
            base.Awake();
            enemyController = GetComponentInParent<EnemyController>();
            enemyVFX = GetComponentInParent<Enemy_VFX>();
        }

        private void EnableCounterWindow()
        {
            enemyVFX.EnableAttackAlert(true);
            enemyController.EnableCounterWindow(true);
        }
        
        private void DisableCounterWindow() 
        {
            enemyVFX.EnableAttackAlert(false);
            enemyController.EnableCounterWindow(false);
        }
    }
}