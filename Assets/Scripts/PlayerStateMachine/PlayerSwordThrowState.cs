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
            skillsManager.SwordThrow.EnableDots(true);
            if (mainCamera == null) mainCamera = Camera.main;
        }

        public override void Update()
        {
            base.Update();
            
            Vector2 dirToMouse = DirectionToMouse();
            
            Player.playerMove.SetVelocity(0, Rb.linearVelocity.y);
            Player.playerMove.HandleFlipEntitySprite(dirToMouse.x);
            skillsManager.SwordThrow.PredictTrajectory(dirToMouse);

            if (Player.playerMove.InputActions.Player.Attack.WasPressedThisFrame())
            {
                Anim.SetBool(ThrowSwordPerformed, true);
                
                skillsManager.SwordThrow.ConfirmTrajectory(dirToMouse);
                skillsManager.SwordThrow.EnableDots(false);
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
            
            skillsManager.SwordThrow.EnableDots(false);
        }

        private Vector2 DirectionToMouse()
        {
            Vector2 playerPosition = Player.transform.position;
            Vector2 worldPosition = mainCamera.ScreenToWorldPoint(Player.playerMove.MousePosition);
            
            return (worldPosition - playerPosition).normalized;
        }
    }
}