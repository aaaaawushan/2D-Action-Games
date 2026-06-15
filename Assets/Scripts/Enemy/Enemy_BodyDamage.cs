using UnityEngine;

public class Enemy_BodyDamage : MonoBehaviour
{
    private Enemy enemy;
    private Enemy_Health enemyHealth;

    [SerializeField] private float bodyDamage = 5f;
    [SerializeField] private float bodyDamageCooldown = 1f;
    [SerializeField] private Vector2 KnockBack = new Vector2(3f, 2f);
    private float lastBodyDamageTime = -999f;
    [SerializeField] private float knockDurationTime = 0.2f;
    private void Awake()
    {
        enemy = GetComponentInParent<Enemy>();
        enemyHealth = GetComponentInParent<Enemy_Health>();
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (Time.time - lastBodyDamageTime < bodyDamageCooldown) return;
        if (!collision.CompareTag("Player")) return;
        if (enemyHealth.IsDead) return;

        //fly enemy‚ªUŒ‚ó‘Ô‚ÌŽžbodaydamage‚ª–³Œø‰»
        Enemy_Flying flyingEnemy = enemy as Enemy_Flying;
        if (flyingEnemy != null && flyingEnemy.isAttack) return;

        IDamageable damageable = collision.GetComponent<IDamageable>();
        if (damageable == null) return;

        damageable.TakeDamage(bodyDamage, 0, ElementType.Fire, enemy.transform);
        Entity playerEntity = collision.GetComponent<Entity>();
        if (playerEntity != null)
        {
            int direction = collision.transform.position.x > enemy.transform.position.x ? 1 : -1;
            playerEntity.ReceiveKnockback(new Vector2(KnockBack.x * direction, KnockBack.y), 0.2f);
        }
        lastBodyDamageTime = Time.time;

    }
}