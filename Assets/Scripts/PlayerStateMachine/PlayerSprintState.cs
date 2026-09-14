using EntityStateMachine;
using PlayerControl;
using UnityEngine;

namespace PlayerStateMachine
{
    public class PlayerSprintState : PlayerState
    {
        public PlayerSprintState(
            PlayerController player, 
            StateMachine stateMachine, 
            string animBoolName) : base(
            player, stateMachine, animBoolName)
        { }

        private float _originalGravityScale;

        public override void Enter()
        {
            base.Enter();
            stateTimer = Player.playerMove.sprintDuration;
            
            _originalGravityScale = Player.playerMove.rb.gravityScale;
            Player.playerMove.rb.gravityScale = 0;
        }
        
        public override void Update()
        {
            base.Update();
            CancelSprintWhenNeeded();
            
            Player.playerMove.SetVelocity(
                Player.playerMove.sprintSpeed * Player.playerMove.FacingDir,
                0);

            if (stateTimer <= 0)
            {
                if (Player.playerCollision.IsGrounded) StateMachine.ChangeState(Player.IdleState);
                else StateMachine.ChangeState(Player.FallState);
            }
            
        }
        
        public override void Exit()
        {
            base.Exit();
            Player.playerMove.SetVelocity(Vector2.zero);
            Player.playerMove.rb.gravityScale = _originalGravityScale;
        }
        
        private void CancelSprintWhenNeeded()
        {
            if (Player.playerCollision.IsWallDetected)
            {
                if (Player.playerCollision.IsGrounded) StateMachine.ChangeState(Player.IdleState);
                else StateMachine.ChangeState(Player.WallSlideState);
            }
        }
    }
}
