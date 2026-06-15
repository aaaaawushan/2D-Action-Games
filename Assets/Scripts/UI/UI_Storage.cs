using UnityEngine;

public class UI_Storage : MonoBehaviour
{
    private Inventory_Player inventory;
    private Inventory_Base storage;


    [SerializeField] private UI_ItemSlotParent inventoryParent;
    [SerializeField] private UI_ItemSlotParent storageParent;

    public void SetupStorage(Inventory_Player inventory, Inventory_Base storage)
    {
        if (this.storage != null)
            this.storage.OnInventoryChange -= UpdateUI;

        this.inventory = inventory;
        this.storage = storage;
        storage.OnInventoryChange += UpdateUI;
        UpdateUI();

        UI_StorageSlot[] storageSlots = GetComponentsInChildren<UI_StorageSlot>();
        foreach (var slot in storageSlots)
            slot.SetStorage(storage);
    }

    private void UpdateUI()
    {
        inventoryParent.UpdateSlots(inventory.itemList);
        storageParent.UpdateSlots(storage.itemList);
    }
}