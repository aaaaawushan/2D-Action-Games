using UnityEngine;

public class Enemy_FlyAttackState : Enemy_FlyingState
{
    private float attackTimer;
    [SerializeField] private float attackCooldown = 0.4f;

    public Enemy_FlyAttackState(Enemy_Flying enemy, StateMachine stateMachine, string stateName)
        : base(enemy, stateMachine, stateName)
    {

    }
    public override void Enter()
    {
        base.Enter();
        rb.linearVelocity = Vector2.zero;
        attackTimer = 0f;
        flyingEnemy.isAttack = true;
    }

    public override void Update()
    {
        base.Update();

        attackTimer += Time.deltaTime;

        if (flyingEnemy.player != null)
            flyingEnemy.HandleFlip(flyingEnemy.player.position.x - flyingEnemy.transform.position.x);

        if (attackTimer >= attackCooldown)
        {
            if (flyingEnemy.PlayerInAttackRange())
            {
                stateMachine.ChangeState(flyingEnemy.flyAttackState);
            }
            else if (flyingEnemy.PlayerInChaseRange())
            {
                stateMachine.ChangeState(flyingEnemy.flyChaseState);
            }
            else
            {
                stateMachine.ChangeState(flyingEnemy.flyIdleState);
            }
        }
    }

    public override void Exit()
    {
        flyingEnemy.isAttack = false;
        base.Exit();
    }

    public override void CallAnimationTrigger()
    {
        DealDamageToPlayer();
    }

    private void DealDamageToPlayer()
    {
        if (flyingEnemy.player == null) return;

        Collider2D hit = Physics2D.OverlapCircle(
            flyingEnemy.transform.position,
            flyingEnemy.attackRange,
           flyingEnemy.WhatIsPlayer
        );

        if (hit != null)
        {
            IDamageable damageable = hit.GetComponent<IDamageable>();
            if (damageable != null)
            {
                float damage = flyingEnemy.stats.GetPhysicalDamage(out bool isCrit, 1f);
                float elementalDamage = flyingEnemy.stats.GetElementalDamage(out ElementType element, 1f);

                damageable.TakeDamage(damage, elementalDamage, element, flyingEnemy.transform);
            }
        }
    }
}