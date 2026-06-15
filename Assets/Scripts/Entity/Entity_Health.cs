using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Entity_Health : MonoBehaviour, IDamageable
{
    [SerializeField] private Slider healthBar;
    protected Entity entity;
    private Entity_VFX entityVfx;
    public Entity_Stats entityStats;

    public float currentHP;
    // [SerializeField] public float MaxHP;
    [SerializeField] public float damage;
    [SerializeField] public bool IsDead;

    [Header("On Damage Knockback")]
    [SerializeField] private Vector2 knockbackPower = new Vector2(2.5f, 2.8f);
    [SerializeField] private Vector2 heavyKnockbackPower = new Vector2(4f, 4.5f);
    [SerializeField] protected float knockbackDuration = .15f;
    private float heavyKnockbackDuration = .2f;
 
    [Header("On Heavy Damage")]
    [SerializeField] private float heavyDamageThreshold = .3f;

    [Header("Health regen")]
    [SerializeField] private float regenInterval = 1f;
    [SerializeField] protected bool canRegenerateHealth = false;

    [Header("Delayed Health Bar")]
    [SerializeField] private Slider delayedHealthBar;
    [SerializeField] private float delayedBarSpeed = 0.5f;
    private float delayedBarTarget;

    [Header("Low Health Warning")]
    [SerializeField] private float warningHpPercent = 0.3f;
    [SerializeField] private float barShakeAmount = 2f;
    [SerializeField] private float flashSpeed = 3f;
    private Vector3 healthBarOriginalPos;
    private Image healthBarFill;
    private Color healthBarOriginalColor;
    private bool warningReady = false;
    [SerializeField] private bool enableLowHealthWarning = true;

    [Header("invincible Info")]
    [SerializeField] private float invincibleTime = 0.3f;
    [SerializeField] private bool isInvincible;

    protected virtual void Awake()
    {
        entityVfx = GetComponent<Entity_VFX>();
        entity = GetComponent<Entity>();
        // healthBar = GetComponentInChildren<Slider>();
        entityStats = GetComponent<Entity_Stats>();
        currentHP = entityStats.GetMaxHealth();
        UpdateHealthBar();

    }
    private void Start()
    {
        delayedBarTarget = 1f;
    }

    protected virtual void Update()
    {
        if (delayedHealthBar == null) return;

        if (delayedHealthBar.value > delayedBarTarget)
        {
            delayedHealthBar.value -= delayedBarSpeed * Time.deltaTime;
            if (delayedHealthBar.value < delayedBarTarget)
                delayedHealthBar.value = delayedBarTarget;
        }

        if (!warningReady) SetupWarning();
        if (healthBar == null) return;

        float percent = currentHP / entityStats.GetMaxHealth();

        if (percent <= warningHpPercent && !IsDead && enableLowHealthWarning)
        {

            float offsetX = Random.Range(-barShakeAmount, barShakeAmount);
            healthBar.transform.localPosition = healthBarOriginalPos + new Vector3(offsetX, 0, 0);

            float alpha = Mathf.Lerp(0.3f, 1f, (Mathf.Sin(Time.time * flashSpeed) + 1f) / 2f);
            healthBarFill.color = new Color(healthBarOriginalColor.r, healthBarOriginalColor.g, healthBarOriginalColor.b, alpha);
        }
        else
        {
            healthBar.transform.localPosition = healthBarOriginalPos;
            if (healthBarFill != null)
                healthBarFill.color = healthBarOriginalColor;
        }
    }
    IEnumerator IsInvincible()
    {
        if (gameObject != null)
        {
            isInvincible = true;
        }

        yield return new WaitForSeconds(invincibleTime);
        isInvincible = false;
    }
    private void SetupWarning()
    {
        if (healthBar == null) return;
        healthBarOriginalPos = healthBar.transform.localPosition;
        healthBarFill = healthBar.fillRect.GetComponent<Image>();
        healthBarOriginalColor = healthBarFill.color;
        warningReady = true;
    }
    public void ChangeHealthBarColor(Color color)
    {
        if (healthBar == null) return;
        healthBar.fillRect.GetComponent<Image>().color = color;
    }
    protected void PlayHitEffects()
    {
        HitEffectManager.instance?.TriggerHitlag();
        HitEffectManager.instance?.CameraShake();
        entityVfx?.PlayOnDamageVfx();
        entityVfx?.CreateOnHitVFX(transform, false, ElementType.None);
    }
    //動画イベントで使ってる
    private void RegenerateHealth()
    {
        if (!canRegenerateHealth)
            return;

        if (IsDead)
            return;

        float regenAmount =
            entityStats.resources.healthRegen.GetValue();

        IncreaseHealth(regenAmount);
    }

    public void IncreaseHealth(float healAmount)
    {
        if (IsDead)
            return;

        float newHealth = currentHP + healAmount;
        float maxHealth = entityStats.GetMaxHealth();

        currentHP = Mathf.Min(newHealth, maxHealth);

        UpdateHealthBar();
    }
    private void UpdateHealthBar()
    {
        if (healthBar == null) return;
        float percent = currentHP / entityStats.GetMaxHealth();
        healthBar.value = percent;
        delayedBarTarget = percent;

    }
    public virtual bool TakeDamage(float damage, float elementalDamage, ElementType element, Transform damageDealer)
    {
        if (IsDead)
            return false;

        if (AttackEvaded())
        {
            return false;
        }
        if (isInvincible) return false;
   
        StartCoroutine(IsInvincible());
        PlayHitEffects();
        Entity_Stats attackerStats = damageDealer.GetComponent<Entity_Stats>();
        float armorReduction = attackerStats != null ? attackerStats.GetArmorReduction() : 0;

        float mitigation = entityStats.GetArmorMitigation(armorReduction);
        float finalDamage = damage * (1 - mitigation);

        float resistance = entityStats.GetElementalResistance(element);
        float elementalDamageTaken = elementalDamage * (1 - resistance);

        Vector2 knockback = CalculateKnockback(finalDamage, damageDealer);
        float duration = CalculateDuration(finalDamage);

        entity?.ReceiveKnockback(knockback, duration);

        ReduceHp(finalDamage + elementalDamageTaken);
        Debug.Log("Elemental damage taken: " + elementalDamageTaken + " element: " + element);

        return true;
    }

    private bool AttackEvaded() => Random.Range(0, 100) < entityStats.GetEvasion();
    public void ReduceHp(float damage)
    {
        //entityVfx.PlayOnDamageVfx();
        currentHP -= damage;
        UpdateHealthBar();

        if (currentHP <= 0)
            Die();
    }
    private void Die()
    {
        IsDead = true;
        entity.EntityDeath();
    }
    protected virtual Vector2 CalculateKnockback(float damage, Transform damageDealer)
    {
        int direction = transform.position.x > damageDealer.position.x ? 1 : -1;

        Vector2 knockback = IsHeavyDamage(damage) ? heavyKnockbackPower : knockbackPower;
        knockback.x = knockback.x * direction;

        return knockback;
    }
    public float GetHealthPercent() => currentHP / entityStats.GetMaxHealth();

    private float CalculateDuration(float damage)
        => IsHeavyDamage(damage) ? heavyKnockbackDuration : knockbackDuration;

    public void SetHealthToPercent(float percent)
    {
        currentHP = entityStats.GetMaxHealth() * Mathf.Clamp01(percent);
        UpdateHealthBar();
    }
    private bool IsHeavyDamage(float damage)
        => damage / entityStats.GetMaxHealth() > heavyDamageThreshold;
}

