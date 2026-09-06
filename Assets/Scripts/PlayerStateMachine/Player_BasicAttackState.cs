using Blueprints;
using PlayerControl;
using UnityEngine;
namespace PlayerStateMachine
{
    public class PlayerBasicAttackState : PlayerState
    {
        public PlayerBasicAttackState(
            PlayerController player,
            StateMachine stateMachine,
            string animBoolName) : base(player, stateMachine, animBoolName)
        { }
        
        private static readonly int AttackAnimIndexParam = Animator.StringToHash("basicAttackIndex");
        private int _comboIndex = 1;
        private int _attackDir;
        private float _attackVelocityTimer;
        private float _lastTimeAttacked;
        private bool _comboAttackQueued;
        private const int FirstComboIndex = 1;
        private const int ComboIndexMax = 3;
        
        public override void Enter()
        {
            base.Enter();
            _comboAttackQueued = false;
    
            if (Player.playerMove.MoveInput.x != 0) _attackDir = ((int)Player.playerMove.MoveInput.x);
            else _attackDir = Player.playerMove.FacingDir;

            // !!! Order matters: _comboIndex can be 4 (transient, post-3rd-hit) until reset here.
            // Reset before reading, or GenerateAttackVelocity() indexes out of bounds.
            CheckAndResetComboIndex();
            GenerateAttackVelocity();
    
            Anim.SetInteger(AttackAnimIndexParam, _comboIndex);
        }
        
        public override void Update()
        {
            base.Update();
            HandleAttackVelocity();
            
            if (Player.playerMove.InputActions.Player.Attack.WasPressedThisFrame())
                QueNextAttack();
            
            if (TriggerCalled) HandleStateExit();
        }
        
        public override void Exit()
        {
            base.Exit();
            _comboIndex++;
            _lastTimeAttacked = Time.time;
        }

        private void HandleStateExit()
        {
            if (_comboAttackQueued)
            {
                Anim.SetBool(AnimBoolName, false);
                Player.EnterAttackStateWithDelay();
            }
            else 
                StateMachine.ChangeState(Player.IdleState);
        }
        
        private void QueNextAttack()
        {
            if (_comboIndex < ComboIndexMax) _comboAttackQueued = true;
        }
        
        private void HandleAttackVelocity()
        {
            _attackVelocityTimer -= Time.deltaTime;
            if (_attackVelocityTimer < 0) 
                Player.playerMove.SetVelocity(0, Rb.linearVelocity.y);
        }
        
        private void GenerateAttackVelocity()
        {
            Vector2 attackVelocity = Player.attackVelocity[_comboIndex - 1];
            
            _attackVelocityTimer = Player.attackVelocityDuration;
            Player.playerMove.SetVelocity(attackVelocity.x * _attackDir, attackVelocity.y);
        }
        
        private void CheckAndResetComboIndex()
        {
            if (Time.time > _lastTimeAttacked + Player.comboAttackResetTime)
            {
                _comboIndex = FirstComboIndex;
                return;
            }
            if (_comboIndex > ComboIndexMax) _comboIndex = FirstComboIndex;
        }
    }
}