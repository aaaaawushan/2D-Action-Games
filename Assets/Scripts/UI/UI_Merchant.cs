using UnityEngine;

public class UI_Merchant : MonoBehaviour
{
    private Inventory_Player inventory;
    private Inventory_Merchant merchant;

    [SerializeField] private UI_ItemSlotParent inventoryParent;
    [SerializeField] private UI_ItemSlotParent merchantParent;

    public void SetupShop(Inventory_Player inventory, Inventory_Merchant merchant)
    {
        if (this.merchant != null)
            this.merchant.OnInventoryChange -= UpdateUI;
        this.inventory = inventory;
        this.merchant = merchant;

        merchant.OnInventoryChange += UpdateUI;
        UpdateUI();

        UI_MerchantSlot[] merchantSlots = GetComponentsInChildren<UI_MerchantSlot>();
        foreach (var slot in merchantSlots)
            slot.SetMerchant(merchant);
    }

    private void UpdateUI()
    {
        inventoryParent.UpdateSlots(inventory.itemList);
        merchantParent.UpdateSlots(merchant.itemList);
    }
}
