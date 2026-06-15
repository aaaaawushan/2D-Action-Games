using Unity.VisualScripting;
using UnityEngine;

public class Enemy_ShadowIdleState : Enemy_ShadowState
{
    public Enemy_ShadowIdleState(Enemy_Shadow shadowEnemy, StateMachine stateMachine, string stateName) : base(shadowEnemy, stateMachine, stateName)
    {
    }

    public override void Update()
    {
        base.Update();
        if (!shadowEnemy.battleStarted) return;

 
        RaycastHit2D hit = enemy.PlayerDetection();
        if (hit.collider != null)
        {
            shadowEnemy.SetPlayer(hit.collider.transform);
            stateMachine.ChangeState(shadowEnemy.shadowChaseState);
        }

    }
}
