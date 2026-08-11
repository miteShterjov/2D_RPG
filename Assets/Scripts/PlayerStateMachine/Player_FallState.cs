using Blueprints;
using PlayerControl;

namespace PlayerStateMachine
{
    public class PlayerFallState : PlayerAiredState
    {
        public PlayerFallState(
            PlayerController player,
            StateMachine stateMachine,
            string animBoolName) : base(player, stateMachine, animBoolName)
        {
        }

        public override void Update()
        {
            base.Update();
            if (Player.playerCollision.IsGrounded)
                StateMachine.ChangeState(Player.playerMove.MoveInput.x != 0 ? Player.MoveState : Player.IdleState);
            if (Player.playerCollision.IsWallDetected) StateMachine.ChangeState(Player.WallSlideState);
        }
    }
}