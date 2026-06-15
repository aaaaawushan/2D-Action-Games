using UnityEngine;
using System;
[CreateAssetMenu(menuName = "RPG Setup/Skill Data", fileName = "Skill data -")]
public class Skill_DataSO : ScriptableObject//一个数据文件,为了可以直接在unity里直接换上不同的技能信息
{
    [Header("Unlock & Upgrade")]
    public int cost;
    public SkillType skillType;
    public UpgradeData upgradeDate;
    public bool unlockedByDefault;

    [Header("Skill description")]
    public string displayName;

    [TextArea]
    public string description;
    public Sprite icon;

}
[Serializable]
public class UpgradeData
{
    public SkillUpgradeType updateType;
    public float cooldown;
    public DamageScaleData damageScale;
}

