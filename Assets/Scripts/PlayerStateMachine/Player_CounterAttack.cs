using Blueprints;
using PlayerControl;
using UnityEngine;

namespace PlayerStateMachine
{
    public class Player_CounterAttack : PlayerState
    {
        private static readonly int CounterAttackPerformed = Animator.StringToHash("counterAttackPerformed");
        private PlayerCombatController playerCombat;
        private bool counterEnemy;

        public Player_CounterAttack(
            PlayerController player,
            StateMachine stateMachine,
            string animBoolName) : base(player, stateMachine, animBoolName)
        {
            playerCombat = player.GetComponent<PlayerCombatController>();
        }

        public override void Enter()
        {
            base.Enter();
            counterEnemy = playerCombat.CounterAttackPerformed();

            if (counterEnemy) playerCombat.PreformAttackEffect();

            Anim.SetBool(CounterAttackPerformed, counterEnemy);
            stateTimer = playerCombat.GetCounterRecoveryDuration();
        }
        
        public override void Update()
        {
            base.Update();
            
            Player.playerMove.SetVelocity(0, Rb.linearVelocity.y);
           
            if (TriggerCalled) StateMachine.ChangeState(Player.IdleState);
            
            if (stateTimer < 0 && !counterEnemy) StateMachine.ChangeState(Player.IdleState);
        }
    }
}