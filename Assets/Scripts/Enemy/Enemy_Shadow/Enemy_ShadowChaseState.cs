using UnityEngine;

public class Enemy_ShadowChaseState : Enemy_ShadowState
{
    public Enemy_ShadowChaseState(Enemy_Shadow shadowEnemy, StateMachine stateMachine, string stateName) : base(shadowEnemy, stateMachine, stateName)
    {
    }

    public override void Update()
    {
        base.Update();

        float dirToPlayer = Mathf.Sign(enemy.player.position.x - enemy.transform.position.x);
        enemy.SetVelocity(dirToPlayer * enemy.battleMoveSpeed, rb.linearVelocity.y);

        float distance = Mathf.Abs(enemy.player.position.x - enemy.transform.position.x);


        if (distance <= enemy.attackDistance) shadowEnemy.RandomAttackState();
        
    }
    public override void Exit()
    {
        base.Exit();
        shadowEnemy.SetVelocity(0, 0);
    }
}
