using EntityStateMachine;
using PlayerControl;
using UnityEngine;

namespace PlayerStateMachine
{
    public class PlayerSwordThrowState : PlayerState
    {
        private static readonly int ThrowSwordPerformed = Animator.StringToHash("swordThrowPerformed");
        private Camera mainCamera;

        public PlayerSwordThrowState(
            PlayerController player, 
            StateMachine stateMachine, 
            string animBoolName) : base(player, stateMachine, animBoolName)
        { }

        public override void Enter()
        {
            base.Enter();
            if (!mainCamera) mainCamera = Camera.main;
        }

        public override void Update()
        {
            base.Update();
            
            Vector2 dirToMouse = DirectionToMouse();
            
            Player.playerMove.SetVelocity(0, Rb.linearVelocity.y);
            Player.playerMove.HandleFlipEntitySprite(dirToMouse.x);

            if (Player.playerMove.InputActions.Player.Attack.WasPressedThisFrame())
            {
                Anim.SetBool(ThrowSwordPerformed, true);
                //todo skill manager create and throw sword
            }

            if (Player.playerMove.InputActions.Player.RangeAttack.WasReleasedThisFrame() || TriggerCalled)
            {
                StateMachine.ChangeState(Player.IdleState);
            }
        }

        public override void Exit()
        {
            base.Exit();
            Anim.SetBool(ThrowSwordPerformed, false);
        }

        private Vector2 DirectionToMouse()
        {
            Vector2 playerPosition = Player.transform.position;
            Vector2 worldPosition = mainCamera.ScreenToWorldPoint(Player.playerMove.MousePosition);
            
            return (worldPosition - playerPosition).normalized;
        }
    }
}