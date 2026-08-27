using Blueprints;
using PlayerControl;
using UnityEngine;

namespace PlayerStateMachine
{
    public class PLayerJumpAttackState : PlayerState
    {
        public PLayerJumpAttackState(
            PlayerController player, 
            StateMachine stateMachine, 
            string animBoolName) : base(player, stateMachine, animBoolName)
        { }
        
        private bool _touchedGround;
        
        private static readonly int JumpAttackTrigger = Animator.StringToHash("jumpAttackTrigger");

        public override void Enter()
        {
            base.Enter();
            _touchedGround = false;
            
            Player.playerMove.SetVelocity(
                Player.jumpAttackVelocity.x * Player.playerMove.FacingDir,
                Player.jumpAttackVelocity.y);
        }

        public override void Update()
        {
            base.Update();
            if (Player.playerCollision.IsGrounded && !_touchedGround)
            {
                _touchedGround = true;
                Anim.SetTrigger(JumpAttackTrigger);
                Player.playerMove.SetVelocity(0, Rb.linearVelocity.y);
            }
            
            if (TriggerCalled && Player.playerCollision.IsGrounded)
                StateMachine.ChangeState(Player.IdleState);
        }
    }
}