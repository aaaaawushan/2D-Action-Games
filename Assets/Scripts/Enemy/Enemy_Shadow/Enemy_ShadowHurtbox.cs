using UnityEngine;

public class Enemy_ShadowHurtbox : MonoBehaviour,IDamageable
{
    private IDamageable parentDamageable;

    private Entity_Health parentHealth;

    void Awake()
    {
        parentHealth = GetComponentInParent<Entity_Health>();
    }

    public bool TakeDamage(float damage, float elementalDamage, ElementType element, Transform damageDealer)
    {
        return parentHealth.TakeDamage(damage, elementalDamage, element, damageDealer);
    }
}
