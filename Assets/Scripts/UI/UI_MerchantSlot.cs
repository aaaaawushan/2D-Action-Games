using UnityEngine.EventSystems;

public class UI_MerchantSlot : UI_ItemSlot
{
    private Inventory_Merchant merchant;

    public void SetMerchant(Inventory_Merchant merchant)
        => this.merchant = merchant;

    public override void OnPointerDown(PointerEventData eventData)
    {
        if (itemInSlot == null) return;

        merchant.BuyItem(itemInSlot);

        ui.itemToolTip.ShowToolTip(false, null);
    }
}