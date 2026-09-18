using EntityStateMachine;
using PlayerControl;
using UnityEngine;

namespace PlayerStateMachine
{
    public abstract class PlayerState : EntityState
    {
        protected readonly PlayerController Player;
        
        private static readonly int YVelocity = Animator.StringToHash("yVelocity");

        protected PlayerState(
            PlayerController player, 
            StateMachine stateMachine, 
            string animBoolName
        ) : base(stateMachine, animBoolName)
        {
            this.Player = player;
        
            Anim = player.Animator;
            Rb = player.playerMove.rb;
            generalStats = player.generalStats;
        }

        public override void Update()
        {
            base.Update();
         
            if (Player.playerMove.InputActions.Player.Sprint.WasPressedThisFrame() && CanSprint()) StateMachine.ChangeState(Player.SprintState);
        }

        protected override void UpdateAnimationParams()
        {
            base.UpdateAnimationParams();
            Anim.SetFloat(YVelocity, Rb.linearVelocity.y);
        }

        private bool CanSprint()
        {
            if (Player.playerCollision.IsWallDetected) return false;
            if (StateMachine.CurrentState == Player.SprintState) return false;
            return true;
        }
    }
}
