using Blueprints;
using EnemyControl;
using EnemyStateMachine;
using UnityEngine;

namespace EnemyNPCs
{
    public class EnemySkeleton : EnemyController, ICounterable
    {
        public bool CanBeCountered { get => cabBeStunned; }
        
        private const string IdleAnimConst = "idle";
        private const string MoveAnimConst = "move";
        private const string AttackAnimConst = "attack";
        private const string BattleAnimConst = "battle";
        private const string StunnedAnimConst = "stun";

        protected override void Awake()
        {
            base.Awake();
            InitEnemyStates();
        }

        private void InitEnemyStates()
        {
            IdleState = new Enemy_IdleState(this, _stateMachine, IdleAnimConst);
            MoveState = new Enemy_MoveState(this, _stateMachine, MoveAnimConst);
            AttackState = new Enemy_AttackState(this, _stateMachine, AttackAnimConst);
            BattleState = new Enemy_BattleState(this, _stateMachine, BattleAnimConst);
            DeathState = new Enemy_DeathState(this, _stateMachine, IdleAnimConst);
            StunnedState = new Enemy_StunnedState(this, _stateMachine, StunnedAnimConst);
        }

        protected void Start()
        {
            _stateMachine.Initialize(IdleState);
        }

        [ContextMenu("Stun Enemy")]
        public void HandleCounterAttack()
        {
            if (!CanBeCountered || _stateMachine.CurrentState != AttackState) return;
            EnableCounterWindow(false);
            _stateMachine.ChangeState(StunnedState);
        }

        
    }
}
