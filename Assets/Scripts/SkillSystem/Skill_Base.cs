using UnityEngine;

public class Skill_Base : MonoBehaviour
{
    public Player_SkillManager skillManager { get; private set; }
    public Player player { get; private set; }
    public DamageScaleData damageScaleData { get; private set; }

    [Header("General details")]
    [SerializeField] protected SkillType skillType;
    [SerializeField] protected SkillUpgradeType updateType;
    [SerializeField] protected float cooldown=1f;
    private float lastTimeUsed;

    protected virtual void Awake()
    {
        skillManager = GetComponent<Player_SkillManager>();
        player = GetComponentInParent<Player>();
        lastTimeUsed = lastTimeUsed - cooldown;
    }
    public virtual void TryUseSkill()
    {

    }
    public void SetSkillUpgrade(UpgradeData upgrade)
    {
        updateType = upgrade.updateType;
        cooldown = upgrade.cooldown;
        damageScaleData = upgrade.damageScale;
    }
    public bool CanUseSkill()
    {
        if (updateType == SkillUpgradeType.None) return false;

        if (OnCooldown())
        {
            Debug.Log("On Cooldown");
            return false;
        }
        return true;
    }

    protected bool OnCooldown() => Time.time < lastTimeUsed + cooldown;

    public void SetSkillOnCooldown() => lastTimeUsed = Time.time;

    public void ResetCooldownBy(float cooldownReduction) => lastTimeUsed = lastTimeUsed + cooldownReduction;

    public void ResetCooldown() => lastTimeUsed = Time.time;

    protected bool Unlocked(SkillUpgradeType upgradeToCheck) => updateType == upgradeToCheck;
}