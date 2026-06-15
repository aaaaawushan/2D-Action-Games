using UnityEngine;
public class Enemy_Flying : Enemy
{
    public Enemy_FlyIdleState flyIdleState;
    public Enemy_FlyChaseState flyChaseState;
    public Enemy_FlyAttackState flyAttackState;
    public Enemy_FlyDeadState flyDeadState;
    public Enemy_FlyTakeHitState flyTakeHitState;

   [Header("Chase Info")]
    public float chaseRange = 8f;
    public float flySpeed = 2f;

    [Header("attack Info")]
    public float attackRange = 2f;
    public bool isAttack;
  

    protected override void Awake()
    {
        base.Awake();
        flyIdleState = new Enemy_FlyIdleState(this, stateMachine, "fly");
        flyChaseState = new Enemy_FlyChaseState(this, stateMachine, "chase");
        flyAttackState = new Enemy_FlyAttackState(this, stateMachine, "attack");
        flyDeadState = new Enemy_FlyDeadState(this, stateMachine, "dead");
        flyTakeHitState = new Enemy_FlyTakeHitState(this, stateMachine, "takeHit");
      
    }

    protected override void Start()
    {
        base.Start();
        rb.gravityScale = 0;
        stateMachine.Initialize(flyIdleState);
    }
    public void EnterTakeHitState()
    {
        stateMachine.ChangeState(flyTakeHitState);
    }
    public override void EntityDeath()
    {
        stateMachine.ChangeState(flyDeadState);
    }
    protected override void HandlePlayerDeath()
    {
        if (flyIdleState != null)
            stateMachine.ChangeState(flyIdleState);
    }
    public override void TryEnterAttackState(Transform player)
    {
        this.player = player;

        if (stateMachine.currentState == flyAttackState)
            return;

        stateMachine.ChangeState(flyChaseState);
    }
    public bool PlayerInChaseRange()
    {
        Collider2D hit = Physics2D.OverlapCircle(transform.position, chaseRange, WhatIsPlayer);
        if (hit != null)
            player = hit.transform;
        return hit != null;
    }

    public bool PlayerInAttackRange()
    {
        return Physics2D.OverlapCircle(transform.position, attackRange, WhatIsPlayer);
    }

    protected override void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, chaseRange);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}