using System;
using System.Collections.Generic;
using UnityEngine;

public class Inventory_Player : Inventory_Base
{
    private Player player;
    public List<Inventory_EquipmentSlot> equipList;
    public int gold = 10;

    [SerializeField] private ItemDataSO healPotionData;
    public event Action OnGoldChange;
    public static Inventory_Player Instance { get; private set; }


    protected override void Awake()
    {
        Instance = this;
        player = GetComponent<Player>();

        if (PlayerPrefs.HasKey("SavedPotion"))
        {
            int count = PlayerPrefs.GetInt("SavedPotion");
            for (int i = 0; i < count; i++)
            {
                AddItem(new Inventory_Item(healPotionData));
            }
            PlayerPrefs.DeleteKey("SavedPotion");
        }

        if (PlayerPrefs.HasKey("SavedGold"))
        {
            gold = PlayerPrefs.GetInt("SavedGold");
            PlayerPrefs.DeleteKey("SavedGold");
            OnGoldChange?.Invoke();
        }
    }
    public bool hasDiamond()
    {
        return itemList.Exists(item => item.itemData.itemType == ItemType.Buyable);
    }
    public void usedDiamond()
    {
        var diamond = itemList.Find(item => item.itemData.itemType == ItemType.Buyable);
        if (diamond != null) RemoveItem(diamond);
    }
    public override void ClearInventory()
    {
        base.ClearInventory();

        foreach (var slot in equipList)
        {
            slot.equipedItem = null;
        }
    }
    public void AddGold(int amount)
    {
        gold += amount;
        OnGoldChange?.Invoke();
    }
    public bool CanUseGold(int amount)
    {
        if (gold < amount) return false;
        gold -= amount;
        OnGoldChange?.Invoke();

        return true;
    }
    public int GetGold()
    {
        return gold;
    }

    public void RemoveGold(int amount)
    {
        gold -= amount;
        if (gold < 0) gold = 0;
        OnGoldChange?.Invoke();
    }
    public void TryEquipItem(Inventory_Item item)
    {
        var inventoryItem = FindItem(item.itemData);
        var matchingSlots = equipList.FindAll(slot => slot.slotType == item.itemData.itemType);

        foreach (var slot in matchingSlots)
        {
            if (slot.HasItem() == false)
            {
                EquipItem(inventoryItem, slot);
                return;
            }
        }
        var slotToReplace = matchingSlots[0];
        var itemToUnequip = slotToReplace.equipedItem;

        UnequipItem(itemToUnequip, slotToReplace != null);
        EquipItem(inventoryItem, slotToReplace);
    }

    private void EquipItem(Inventory_Item itemToEquip, Inventory_EquipmentSlot slot)
    {
        float savedHealthPercent = player.health.GetHealthPercent();
        slot.equipedItem = itemToEquip;
        slot.equipedItem.AddModifiers(player.stats);
        player.health.SetHealthToPercent(savedHealthPercent);

        RemoveItem(itemToEquip);
    }
    public void UnequipItem(Inventory_Item itemToUnequip, bool replacingItem = false)
    {
        if (CanAddItem(itemToUnequip) == false && replacingItem == false)
        {
            Debug.Log("No space!");
            return;
        }
        float savedHealthPrecent = player.health.GetHealthPercent();
        var slotToUnequip = equipList.Find(slot => slot.equipedItem == itemToUnequip);

        if (slotToUnequip != null) slotToUnequip.equipedItem = null;
        itemToUnequip.RemoveModifiers(player.stats);

        player.health.SetHealthToPercent(savedHealthPrecent);
        AddItem(itemToUnequip);
    }
}