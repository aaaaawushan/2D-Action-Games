using UnityEngine;

public class Enemy_FlyTakeHitState : Enemy_FlyingState
{
    public Enemy_FlyTakeHitState(Enemy_Flying enemy, StateMachine stateMachine, string stateName)
        : base(enemy, stateMachine, stateName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        rb.linearVelocity = new Vector2(1.1f, 1.5f); ;
    }

    public override void CallAnimationTrigger()
    {
        if (flyingEnemy.PlayerInChaseRange())
            stateMachine.ChangeState(flyingEnemy.flyChaseState);
        else
            stateMachine.ChangeState(flyingEnemy.flyIdleState);
    }
}