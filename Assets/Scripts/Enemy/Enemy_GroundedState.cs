using UnityEngine;

public class Enemy_GroundedState : EnemyState
{
    public Enemy_GroundedState(Enemy enemy, StateMachine stateMachine, string stateName) : base(enemy, stateMachine, stateName)
    {
    }

    public override void Enter()
    {
        base.Enter();

    }
    public override void Update()
    {
        base.Update();
        RaycastHit2D hit = enemy.PlayerDetection();
        if (hit.collider != null)
            enemy.TryEnterAttackState(hit.transform);
    }

}
