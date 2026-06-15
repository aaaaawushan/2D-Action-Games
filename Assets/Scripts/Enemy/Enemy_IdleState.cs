using UnityEngine;

public class Enemy_IdleState : Enemy_GroundedState
{
    public Enemy_IdleState(Enemy enemy, StateMachine stateMachine, string stateName) : base(enemy, stateMachine, stateName)
    {
    }
    public override void Enter()
    {
        base.Enter();
        stateTimer = enemy.idleTime;
        enemy.SetVelocity(0, 0);
    }
    public override void Update()
    {
        base.Update();

        if (stateTimer<0)
        {
            stateMachine.ChangeState(enemy.moveState);
        }
    }
}
