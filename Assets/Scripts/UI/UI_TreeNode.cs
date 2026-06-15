using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System;

public class UI_TreeNode : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler
{
    private UI ui;
    private RectTransform rect;//这是不同技能icon的位置所以在这纴E惨匦露ㄒ丒
    private UI_SkillTree skillTree;
    private UI_TreeConnectHandler connectHandler;

    [Header("Unlock details")]
    public UI_TreeNode[] neededNodes;
    public UI_TreeNode[] conflictNodes;
    public bool isUnlocked;
    public bool isLocked;

    [Header("Skill details")]
    [SerializeField] public UnityEngine.UI.Image skillIcon;
    public Skill_DataSO skillData;
    [SerializeField] private string skillName;
    [SerializeField] private int skillcost;
    [SerializeField] private Color skillLockedColor;


    private void OnValidate()
    {
        if (skillData == null) return;

        skillName = skillData.displayName;
        skillcost = skillData.cost;
        skillIcon.sprite = skillData.icon;
        gameObject.name = "UI_TreeNode - " + skillData.displayName;
    }
    private void Awake()//每次启动竵E耰con颜色
    {
        skillTree = GetComponentInParent<UI_SkillTree>();
        ui = GetComponentInParent<UI>();
        rect = GetComponent<RectTransform>();
        connectHandler = GetComponent<UI_TreeConnectHandler>();

        UpdateIconColor(skillLockedColor);

    }
    private void Start()
    {
        if (skillData.unlockedByDefault) Unlock();
    }
    public void Refund()
    {
        isUnlocked = false;
        isLocked = false;
        UpdateIconColor(skillLockedColor);

        skillTree.AddSkillPoints(skillData.cost);
        connectHandler.UnlockConnectionImage(false);

        skillTree.skillManager.GetSkillByType(skillData.skillType).SetSkillUpgrade(skillData.upgradeDate);

        // skill manager and reset skill
    }
    private void Unlock()
    {
        isUnlocked = true;
        UpdateIconColor(Color.white);
        skillTree.RemoveSkillPoints(skillData.cost);
        LockConflictNodes();
        connectHandler.UnlockConnectionImage(true);
    }
    private bool CanBeUnlocked()
    {
        if (isLocked || isUnlocked) { return false; }

        if (skillTree.EnoughSkillPoints(skillData.cost) == false)
        {
            return false;
        }

        foreach (var node in neededNodes)
        {
            if (node.isUnlocked == false)
                return false;
        }

        foreach (var node in conflictNodes)
        {
            if (node.isUnlocked)
                return false;
        }

        return true;
    }

    private void LockConflictNodes()
    {
        foreach (var node in conflictNodes)
        {
            node.isLocked = true;
            node.LockChildNodes();
        }
    }

    public void LockChildNodes(int depth = 0)
    {
        if (depth > 100)
        {
            Debug.LogError($"紒E獾窖芬茫〗诘悖簕gameObject.name}");
            return;
        }
        isLocked = true;

        foreach (var node in connectHandler.GetChildNode())
        {
            if (!node.isLocked) // 已锁定说明已访问过，跳过防止循环
                node.LockChildNodes(depth + 1);
        }
    }
    private void UpdateIconColor(Color color)
    {
        if (skillIcon == null)
        {
            return;
        }
        skillIcon.color = color;
    }
    public void OnPointerDown(PointerEventData eventData)
    {

        if (CanBeUnlocked()) Unlock();
        else if (isLocked)
        {
            ui.skillToolTip.LockedSkillEffect();
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        ui.skillToolTip.ShowToolTip(true, rect, this);

        if (isUnlocked || isLocked)
            return;

        Color color = Color.white * .9f; color.a = 1;
        UpdateIconColor(color);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        ui.skillToolTip.ShowToolTip(false, rect);

        if (isUnlocked || isLocked)
            return;

        UpdateIconColor(skillLockedColor);

    }


}
