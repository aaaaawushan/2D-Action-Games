using Unity.VisualScripting;
using UnityEngine;

public class Enemy_ShadowAttack1State : Enemy_ShadowState
{
    public Enemy_ShadowAttack1State(Enemy_Shadow shadowEnemy, StateMachine stateMachine, string stateName) : base(shadowEnemy, stateMachine, stateName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        rb.linearVelocity = Vector2.zero;
        shadowEnemy.attackFinished = false;
        shadowEnemy.cooldownTime =0.4f;

    }
    public override void Update()
    {
        base.Update();
        if(shadowEnemy.attackFinished) shadowEnemy.cooldownTime -= Time.deltaTime;

        if (shadowEnemy.cooldownTime <= 0)stateMachine.ChangeState(shadowEnemy.shadowChaseState);

    }
    public override void CallAnimationTrigger()
    {
        shadowEnemy.attackFinished = true;
        shadowEnemy.SetVelocity(0, 0);

    }
    public override void Exit()
    {
        base.Exit();
    }

}
