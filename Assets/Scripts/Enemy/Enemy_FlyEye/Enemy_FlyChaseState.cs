using UnityEngine;
public class Enemy_FlyChaseState : Enemy_FlyingState
{

    public Enemy_FlyChaseState(Enemy_Flying enemy, StateMachine stateMachine, string stateName)
        : base(enemy, stateMachine, stateName)
    {
       
    }

    public override void Enter()
    {
        base.Enter();
    }
    public override void Update()
    {
        base.Update();

        if (!flyingEnemy.PlayerInChaseRange())
        {
            stateMachine.ChangeState(flyingEnemy.flyIdleState);
            return;
        }

        if (flyingEnemy.PlayerInAttackRange())
        {
            stateMachine.ChangeState(flyingEnemy.flyAttackState);
            return;
        }

        Vector2 dir = (flyingEnemy.player.position - flyingEnemy.transform.position).normalized;
        flyingEnemy.SetVelocity(dir.x * flyingEnemy.flySpeed, dir.y * flyingEnemy.flySpeed);
        flyingEnemy.HandleFlip(dir.x);
    }
}