using Unity.IO.LowLevel.Unsafe;
using UnityEngine;

public class Enemy_MoveState : Enemy_GroundedState
{
    public Enemy_MoveState(Enemy enemy, StateMachine stateMachine, string stateName) : base(enemy, stateMachine, stateName)
    {
    }
    public override void Enter()
    {
        base.Enter();
    }
    public override void Update()
    {
        base.Update();
        enemy.SetVelocity(enemy.moveSpeed * enemy.facedir, rb.linearVelocity.y);
        if (enemy.Isgrounded == false || enemy.Iswall)
        {
            enemy.Flip();
            stateMachine.ChangeState(enemy.idleState);
        }
    }
}
 