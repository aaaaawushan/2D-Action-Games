using UnityEngine;

public class Enemy_AttackState : EnemyState
{
    public Enemy_AttackState(Enemy enemy, StateMachine stateMachine, string stateName) : base(enemy, stateMachine, stateName)
    {
    }
    public override void Enter()
    {
        base.Enter();
        enemy.SetVelocity(0, 0);
        SynAttackSpeed();
    }
    public override void Update()
    {
        base.Update();


        if (triggerCalled)
        {
            stateMachine.ChangeState(enemy.battleState);
        }
    }
}
