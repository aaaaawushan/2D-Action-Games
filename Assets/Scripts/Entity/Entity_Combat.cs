using UnityEngine;

public class Entity_Combat : MonoBehaviour
{
    private Entity_VFX vfx;
    private Entity_Stats stats;
    public DamageScaleData basicAttackScale;
    //private float Damage = 10f;
    //  public Collider2D[] targetColliders;
    [Header("Target info")]
    [SerializeField] private LayerMask WhatIsTarget;
    [SerializeField] private Transform targetCheck;
    [SerializeField] private float targetCheckRadius;

    [Header("Status effect details")]
    //   [SerializeField] private float defaultDuration = 3f;
    [SerializeField] private float chillSlowMultiplier = .2f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Awake()
    {
        vfx = GetComponent<Entity_VFX>();
        stats = GetComponent<Entity_Stats>();
    }

    public void PerformAttack()
    {
        Debug.Log("PerformAttack called");

        GetDetectedColliders();
        foreach (var target in GetDetectedColliders())
        {
            IDamageable damageable = target.GetComponent<IDamageable>();

            if (damageable == null)
                continue; // skip target, go to next target

            ElementalEffectData effectData = new ElementalEffectData(stats, basicAttackScale);
            float elementalDamage = stats.GetElementalDamage(out ElementType element, .6f);
            float damage = stats.GetPhysicalDamage(out bool isCrit, 1.5f);
            Debug.Log("Hit target: " + target.name + " damage: " + damage);
            bool targetGotHit = damageable.TakeDamage(damage, elementalDamage, element, transform);

            if (targetGotHit)
            {
                vfx.CreateOnHitVFX(target.transform, isCrit, element);
            }
            if (element != ElementType.None)
                target.GetComponent<Entity_StatusHandler>().ApplyStatusEffect(element, effectData);
        }
    }

    private Collider2D[] GetDetectedColliders()
    {
        return Physics2D.OverlapCircleAll(targetCheck.position, targetCheckRadius, WhatIsTarget);
    }
    private void OnDrawGizmos()
    {
        Gizmos.DrawSphere(targetCheck.position, targetCheckRadius);
    }
}
