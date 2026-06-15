using UnityEngine;

public class Enemy_ShadowState : EnemyState
{
    protected Enemy_Shadow shadowEnemy;
    public Enemy_ShadowState(Enemy_Shadow shadowEnemy, StateMachine stateMachine, string stateName) : base(shadowEnemy, stateMachine, stateName)
    {
        this.shadowEnemy = shadowEnemy;
    }
    public override void Enter()
    {
        base.Enter();

    }
    public override void Update()
    {

    }

}
