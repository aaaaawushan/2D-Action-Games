using UnityEngine;

public class Enemy_FlyingState : EnemyState
{
    protected Enemy_Flying flyingEnemy;

    public Enemy_FlyingState(Enemy_Flying enemy, StateMachine stateMachine, string stateName)
        : base(enemy, stateMachine, stateName)
    {
        this.flyingEnemy = enemy;
    }
    public override void Enter()
    {
        base.Enter();
        
    }
    public override void Update()
    {
    }
}
