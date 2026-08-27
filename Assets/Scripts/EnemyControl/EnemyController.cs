using Blueprints;
using EnemyStateMachine;
using EntityControl;
using UnityEngine;

namespace EnemyControl
{
    public class EnemyController : EntityController
    {
        public Enemy_IdleState IdleState;
        public Enemy_MoveState MoveState;
        public Enemy_AttackState AttackState;
        public Enemy_BattleState BattleState;

        public EnemyMoveController enemyMove;
        public EnemyCollisionController enemyCollision;
        
        protected override void Awake()
        {
            base.Awake();
            _stateMachine = new StateMachine();

            enemyMove = GetComponent<EnemyMoveController>();
            enemyCollision = GetComponent<EnemyCollisionController>();
        }
        
        protected virtual void Update() => _stateMachine.UpdateActiveState();
    }
}
