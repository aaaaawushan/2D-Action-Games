using UnityEngine;

public class Player_Health : Entity_Health
{
    private Player player => GetComponent<Player>();
    public bool takeDamage { get; private set; }
    protected override void Awake()
    {
        base.Awake();
    }
    public void UpdateManaBar()
    {
        if (player.manaBar == null) return;
        float percent = player.currentMana / player.maxMana;
        player.manaBar.value = percent;
    }

    public override bool TakeDamage(float damage, float elementalDamage, ElementType element, Transform damageDealer)
    {
        bool hit = base.TakeDamage(damage, elementalDamage, element, damageDealer);
        if (hit) takeDamage = true;
        return hit;
    }
    public void ResetDamageFrame()
    {
        takeDamage = false;
    }
}
