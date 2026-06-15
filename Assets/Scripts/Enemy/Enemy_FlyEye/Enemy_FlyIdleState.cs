using UnityEngine;

public class Enemy_FlyIdleState : Enemy_FlyingState
{
    public Enemy_FlyIdleState(Enemy_Flying enemy, StateMachine stateMachine, string stateName) : base(enemy, stateMachine, stateName)
    {
        
    }
    public override void Enter()
    {
        base.Enter();
        enemy.SetVelocity(0, 0);
    }

    public override void Update()
    {
        base.Update();
        if (flyingEnemy.PlayerInChaseRange())
            stateMachine.ChangeState(flyingEnemy.flyChaseState);
        else
            return;
    }

}
