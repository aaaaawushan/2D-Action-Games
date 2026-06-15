using UnityEngine;

public class Enemy_ShadowAttack3State : Enemy_ShadowState
{
    private Vector3 attack3StartPos;
    public Enemy_ShadowAttack3State(Enemy_Shadow shadowEnemy, StateMachine stateMachine, string stateName) : base(shadowEnemy, stateMachine, stateName)
    {
    }
    public override void Enter()
    {
        base.Enter();
        rb.linearVelocity = Vector2.zero;
        shadowEnemy.attackFinished = false;
        shadowEnemy.cooldownTime = 0.3f;

        attack3StartPos = shadowEnemy.transform.position;
        shadowEnemy.GetComponent<Collider2D>().enabled = false;
    }
    public override void Update()
    {
        base.Update();

        if (!shadowEnemy.attackFinished)
            rb.linearVelocity = new Vector2(shadowEnemy.facedir * shadowEnemy.attack3RollSpeed, rb.linearVelocity.y);

        shadowEnemy.KeepWithinBounds();

        if (shadowEnemy.attackFinished) shadowEnemy.cooldownTime -= Time.deltaTime;
        if (shadowEnemy.cooldownTime <= 0) stateMachine.ChangeState(shadowEnemy.shadowChaseState);
    }
    public override void CallAnimationTrigger()
    {
        shadowEnemy.attackFinished = true;
        shadowEnemy.SetVelocity(0, 0);
    }
    public override void Exit()
    {
        base.Exit();
        Collider2D col = shadowEnemy.GetComponent<Collider2D>();
        if (Physics2D.OverlapBox(shadowEnemy.transform.position, col.bounds.size, 0, shadowEnemy.WallMask))
        {
            shadowEnemy.transform.position = attack3StartPos;
        }

        col.enabled = true;
    }

}
