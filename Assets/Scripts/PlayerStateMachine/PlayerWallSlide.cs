using EntityStateMachine;
using PlayerControl;
using UnityEngine;

namespace PlayerStateMachine
{
    public class PlayerWallSlide : PlayerState
    {
        private const float WallSlideVisualOffset = 0.125f;
        private Vector3 originalAnimatorPosition;

        public PlayerWallSlide(
            PlayerController player, 
            StateMachine stateMachine, 
            string animBoolName) : base(player, stateMachine, animBoolName)
        { }

        public override void Enter()
        {
            base.Enter();
            Transform animatorTransform = Player.Animator.transform;
            originalAnimatorPosition = animatorTransform.localPosition;
            animatorTransform.localPosition += Vector3.right * WallSlideVisualOffset;
        }
        
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

        public override void Exit()
        {
            Player.Animator.transform.localPosition = originalAnimatorPosition;
            base.Exit();
        }

        private void HandleWallSlide()
        {
            if (PlayerMove.MoveInput.y < 0) PlayerMove.SetVelocity(PlayerMove.MoveInput.x,Rb.linearVelocity.y);
            else PlayerMove.SetVelocity(PlayerMove.MoveInput.x, Rb.linearVelocity.y * PlayerMove.wallSlideSlowMultiplier);
        }
    }
}
