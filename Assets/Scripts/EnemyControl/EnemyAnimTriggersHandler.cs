using EntityControl;
using UnityEngine;

namespace EnemyControl
{
    public class EnemyAnimTriggersHandler : EntityAnimTriggersHandler
    {
        private EnemyController enemyController;
        private EnemyVFX enemyVFX;

        protected override void Awake()
        {
            base.Awake();
            enemyController = GetComponentInParent<EnemyController>();
            enemyVFX = GetComponentInParent<EnemyVFX>();
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