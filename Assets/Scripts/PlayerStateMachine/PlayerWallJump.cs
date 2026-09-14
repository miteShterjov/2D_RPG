using EntityStateMachine;
using PlayerControl;

namespace PlayerStateMachine
{
    public class PlayerWallJump : PlayerState
    {
        public PlayerWallJump(
            PlayerController player, 
            StateMachine stateMachine, 
            string animBoolName) : base(player, stateMachine, animBoolName)
        { }
        
        private PlayerMoveController PlayerMove => Player.playerMove;

        public override void Enter()
        {
            base.Enter();
            PlayerMove.SetVelocity(
                PlayerMove.wallJumpAngle.x * -PlayerMove.FacingDir,
                PlayerMove.wallJumpAngle.y);
        }
    
        public override void Update()
        {
            base.Update();
        
            if (Rb.linearVelocity.y < 0) StateMachine.ChangeState(Player.FallState);
        
            if (Player.playerCollision.IsWallDetected) StateMachine.ChangeState(Player.WallSlideState);
        }
    }
}
