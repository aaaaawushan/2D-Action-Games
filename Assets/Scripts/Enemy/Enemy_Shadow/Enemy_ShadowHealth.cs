using UnityEngine;

public class Enemy_ShadowHealth : Enemy_Health
{
    private Enemy_Shadow shadowEnemy;
    private Animator anim;

    [Header("Phase Transition")]
    [SerializeField] private float phaseKnockbackPower = 5f;
   

    protected override void Awake()
    {
        base.Awake();
        shadowEnemy = GetComponent<Enemy_Shadow>();
        anim = GetComponentInChildren<Animator>();
    }

    public override bool TakeDamage(float damage, float elementalDamage, ElementType element, Transform damageDealer)
    {
        Debug.Log("isInvulnerable: " + shadowEnemy.isInvulnerable);
        if (shadowEnemy.isInvulnerable) return false;

        bool wasHit = base.TakeDamage(damage, elementalDamage, element, damageDealer);
        Debug.Log("HP percent: " + GetHealthPercent());
        if (!wasHit) return false;

        PlayHitEffects();

        if (!shadowEnemy.isPhase2 && GetHealthPercent() <= 0.7f)
        {
            shadowEnemy.isPhase2 = true;

            shadowEnemy.rb.linearVelocity = Vector2.zero;
            Vector2 dir = (transform.position - damageDealer.position).normalized;
            entity.ReceiveKnockback(new Vector2(dir.x * phaseKnockbackPower, 0f), 0.6f);

            shadowEnemy.EnterPhaseTransition();

            AudioSource audio = shadowEnemy.GetComponent<Enemy_ShadowHealth>().enemyAudio;
            if (audio != null && shadowEnemy.phaseTransitionSFX != null)
            {
                audio.PlayOneShot(shadowEnemy.phaseTransitionSFX, 0.8f);
            }
        }

        if (IsDead)
        {
            shadowEnemy.EntityDeath();
            return true;
        }


        return true;
    }
    public void StartRegen()
    {
        canRegenerateHealth = true;
        InvokeRepeating("RegenerateHealth", 1f, 3f);
    }
}
