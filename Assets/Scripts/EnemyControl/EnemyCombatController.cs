using System.Collections;
using EntityControl;
using UnityEngine;

namespace EnemyControl
{
    public class EnemyCombatController : EntityCombatController
    {
        private EnemyController enemyController;
        private EnemyMoveController enemyMove;
        private EnemyCollisionController enemyCollision;

        protected override void Awake()
        {
            base.Awake();
            enemyController = GetComponent<EnemyController>();
            enemyMove = GetComponent<EnemyMoveController>();
            enemyCollision = GetComponent<EnemyCollisionController>();
        }

        public override void PreformAttack()
        {
            if (!enemyController.IsAttackStateActive) return;
            base.PreformAttack();
        }

        protected override IEnumerator SlowDownEntityCo(float duration, float slowMultiplier)
        {
            float originalMoveSpeed = enemyMove.moveSpeed;
            float originalBattleSpeed = enemyController.battleMoveSpeed;
            float originalAnimSpeed = enemyMove.moveAnimSpeedMultiplier;

            float speedMultiplier = 1 - slowMultiplier;

            enemyMove.moveSpeed *= speedMultiplier;
            enemyController.battleMoveSpeed *= slowMultiplier;
            enemyMove.moveAnimSpeedMultiplier *= speedMultiplier;

            yield return new WaitForSeconds(duration);

            enemyMove.moveSpeed = originalMoveSpeed;
            enemyController.battleMoveSpeed = originalBattleSpeed;
            enemyMove.moveAnimSpeedMultiplier = originalAnimSpeed;
        }
    }
}