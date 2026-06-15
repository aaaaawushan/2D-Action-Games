using UnityEngine;
using UnityEngine.UI;

public class UI : MonoBehaviour
{
    public UI_SkillToolTip skillToolTip { get; private set; }
    public UI_SkillTree skillTreeUI { get; private set; }
    public UI_ItemToolTip itemToolTip { get; private set; }

    public UI_Inventory inventoryUI { get; private set; }
    public UI_Storage storageUI { get; private set; }
    public UI_Merchant merchantUI { get; private set; }
    public UI_Dialogue dialogueUI { get; private set; }

    public bool skillTreeEnabled { get; private set; }
    public bool inventoryEnabled { get; private set; }

   
    private void Awake()
    {
        skillToolTip = GetComponentInChildren<UI_SkillToolTip>();
       
        itemToolTip = GetComponentInChildren<UI_ItemToolTip>();

        skillTreeUI = GetComponentInChildren<UI_SkillTree>(true);
        inventoryUI = GetComponentInChildren<UI_Inventory>(true);
        storageUI = GetComponentInChildren<UI_Storage>(true);
        merchantUI = GetComponentInChildren<UI_Merchant>(true);
        dialogueUI = GetComponentInChildren<UI_Dialogue>(true);
    }

    private void Start()
    {
        skillTreeEnabled = false;
        skillTreeUI.gameObject.SetActive(false);
        skillToolTip.ShowToolTip(false, null);
        inventoryUI.gameObject.SetActive(false);

    }

    public void ToggleSkillTreeUI()
    {
        skillTreeEnabled = !skillTreeEnabled;
        skillTreeUI.gameObject.SetActive(skillTreeEnabled);
        skillToolTip.ShowToolTip(false, null);
    }
    public void ToggleInventoryUI()
    {
        inventoryEnabled = !inventoryEnabled;
        inventoryUI.gameObject.SetActive(inventoryEnabled);
        itemToolTip.ShowToolTip(false, null);
    }
}