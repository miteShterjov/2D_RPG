using Blueprints;
using EnemyControl;
using EnemyStateMachine;
using UnityEngine;

public class Enemy_StunnedState : EnemyState
{
    public Enemy_StunnedState(
        EnemyController enemy, 
        StateMachine stateMachine, 
        string animBoolName) : base(enemy, stateMachine, animBoolName)
    { }

    public override void Enter()
    {
        base.Enter();
        
        Enemy.GetComponent<Enemy_VFX>().EnableAttackAlert(false);
        Enemy.EnableCounterWindow(false);
        
        stateTimer = Enemy.stunnedDuration;
        Rb.linearVelocity = new Vector2(Enemy.stunnedVelocity.x * -Enemy.enemyMove.FacingDir, Enemy.stunnedVelocity.y);
    }

    public override void Update()
    {
        base.Update();
        if (stateTimer < 0) StateMachine.ChangeState(Enemy.IdleState);
    }
}
