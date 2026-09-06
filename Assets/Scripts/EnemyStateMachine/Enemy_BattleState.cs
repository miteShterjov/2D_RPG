using System;
using EnemyControl;
using PlayerControl;
using UnityEngine;
using StateMachine = Blueprints.StateMachine;

namespace EnemyStateMachine
{
    public class Enemy_BattleState : EnemyState
    {
        public Enemy_BattleState(
            EnemyController enemy, 
            StateMachine stateMachine, 
            string animBoolName) : base(enemy, stateMachine, animBoolName)
        {
        }
        
        private Transform player;
        private float lastTimeWasInBattle;

        public override void Enter()
        {
            base.Enter();
            UpdateBattleTimer();

            player = Enemy.player;

            if (player == null)
                return;

            var playerHealth = player.GetComponentInParent<PlayerHealthController>();

            if (playerHealth != null && playerHealth.isDead)
            {
                StateMachine.ChangeState(Enemy.IdleState);
                return;
            }

            if (ShouldEnemyRetreat())
            {
                Rb.linearVelocity = new Vector2(
                    Enemy.retreatVelocity.x * -DirectionToPlayer(),
                    Enemy.retreatVelocity.y
                );

                Enemy.enemyMove.FlipEntitySprite(DirectionToPlayer());
            }
        }

        public override void Update()
        {
            base.Update();
    
            if (player == null)
            {
                StateMachine.ChangeState(Enemy.IdleState);
                return;
            }

            if (Enemy.enemyCollision.isPlayerDetected)
                UpdateBattleTimer();

            if (IsPlayerCloseEnough() && Enemy.enemyCollision.isPlayerDetected)
            {
                StateMachine.ChangeState(Enemy.AttackState);
                return;
            }

            if (BattleTimeIsOver())
            {
                StateMachine.ChangeState(Enemy.IdleState);
                return;
            }

            Enemy.enemyMove.SetVelocity(Enemy.battleMoveSpeed * DirectionToPlayer(), Enemy.Rb.linearVelocity.y);
        }

        private bool ShouldEnemyRetreat() => DistanceToPlayer() < Enemy.minRetreatDistance;
        
        private void UpdateBattleTimer() => lastTimeWasInBattle = Time.time;

        private bool BattleTimeIsOver() => Time.time > lastTimeWasInBattle + Enemy.battleTimeDuration;

        private int DirectionToPlayer()
        {
            if (player == null) return 0;
            return player.position.x > Enemy.transform.position.x ? 1 : -1;
        }

        private bool IsPlayerCloseEnough() => DistanceToPlayer() < Enemy.attackDistance;

        private float DistanceToPlayer()
        {
            return player == null ? float.MaxValue : Math.Abs(player.position.x - Enemy.transform.position.x);
        }
    }
}
