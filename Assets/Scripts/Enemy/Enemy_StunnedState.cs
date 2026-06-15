using UnityEngine;

public class Enemy_StunnedState : EnemyState
{
    public Enemy_StunnedState(Enemy enemy, StateMachine stateMachine, string stateName) : base(enemy, stateMachine, stateName)
    {
    }
    public override void Enter()
    {
        base.Enter();
        rb.linearVelocity = new Vector2(enemy.stunnedVelocity.x * -enemy.facedir, enemy.stunnedVelocity.y);
        stateTimer = enemy.stunnedDuration;

    }
    public override void Update()
    {
        base.Update();
        if (stateTimer < 0)
        {
            stateMachine.ChangeState(enemy.idleState);
        }
    }
}
