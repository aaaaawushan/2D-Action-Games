using UnityEngine;
using UnityEngine.EventSystems;

public class UI_EquipSlot : UI_ItemSlot//装备栏的UI格子（继承UI_ItemSlot）
{
    public ItemType slotType;

    private void OnValidate()// // OnValidate = Inspector里改值时自动执行
    {
        gameObject.name = "UI EquipmentSlot - " + slotType.ToString();
    }
    public override void OnPointerDown(PointerEventData eventData)
    {
        if (itemInSlot == null)
            return;

        inventory.UnequipItem(itemInSlot);
    }
}