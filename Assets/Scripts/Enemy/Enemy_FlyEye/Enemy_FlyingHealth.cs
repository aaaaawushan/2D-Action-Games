using UnityEngine;

public class Enemy_FlyingHealth : Enemy_Health
{
    private Enemy_Flying flyingEnemy;
    private Animator anim;

    protected override void Awake()
    {
        base.Awake();
        flyingEnemy = GetComponent<Enemy_Flying>();
        anim = GetComponentInChildren<Animator>();
    }

    public override bool TakeDamage(float damage, float elementalDamage, ElementType element, Transform damageDealer)
    {
        bool wasHit = base.TakeDamage(damage, elementalDamage, element, damageDealer);
        if (!wasHit) return false;


        PlayHitEffects();

        if (IsDead)
        {
            return true;
        }
        flyingEnemy.EnterTakeHitState();
        return true;
    }
}
