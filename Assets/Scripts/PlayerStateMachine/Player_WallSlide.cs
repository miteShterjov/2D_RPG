using Blueprints;
using PlayerControl;

namespace PlayerStateMachine
{
    public class PlayerWallSlide : PlayerState
    {
        public PlayerWallSlide(
            PlayerController player, 
            StateMachine stateMachine, 
            string animBoolName) : base(player, stateMachine, animBoolName)
        { }
        
        private PlayerCollisionController PlayerCollision => Player.playerCollision;
        private PlayerMoveController PlayerMove => Player.playerMove;

        public override void Update()
        {
            base.Update();
            HandleWallSlide();
        
            if (PlayerMove.InputActions.Player.Jump.WasPressedThisFrame()) StateMachine.ChangeState(Player.WallJump);
        
            if (!PlayerCollision.IsWallDetected) StateMachine.ChangeState(Player.FallState);
        
            if (PlayerCollision.IsGrounded)
            {
                StateMachine.ChangeState(Player.IdleState);
                PlayerMove.FlipEntitySprite();
            }

        }

        private void HandleWallSlide()
        {
            if (PlayerMove.MoveInput.y < 0) PlayerMove.SetVelocity(PlayerMove.MoveInput.x,Rb.linearVelocity.y);
            else PlayerMove.SetVelocity(PlayerMove.MoveInput.x, Rb.linearVelocity.y * PlayerMove.wallSlideSlowMultiplier);
        }
    }
}
