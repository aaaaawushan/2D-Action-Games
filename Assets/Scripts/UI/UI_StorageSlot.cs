using UnityEngine;
using UnityEngine.EventSystems;

public class UI_StorageSlot : UI_ItemSlot
{
    private Inventory_Base storage;
    public enum StoragesSlotType { StoragesSlot, PlayerInventorySlot }
    public StoragesSlotType slotType;

    public void SetStorage(Inventory_Base storage) => this.storage = storage;

    public override void OnPointerDown(PointerEventData eventData)
    {
        if (itemInSlot == null)
            return;

        if (slotType == StoragesSlotType.StoragesSlot)
            (storage as Inventory_Storage)?.FromStorageToPlayer(itemInSlot);
        if (slotType == StoragesSlotType.PlayerInventorySlot)
            (storage as Inventory_Storage)?.FromPlayerToStorage(itemInSlot);

        ui.itemToolTip.ShowToolTip(false, null);
    }
}