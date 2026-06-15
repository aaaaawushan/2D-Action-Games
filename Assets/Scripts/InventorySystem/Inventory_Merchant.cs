using System.Collections.Generic;
using UnityEngine;

public class Inventory_Merchant : Inventory_Base
{
    private Inventory_Player playerInventory;
    [SerializeField] private List<BuyableItemDataSo> shopItems; // ��Inspector����

    protected override void Awake()
    {
        base.Awake();
        foreach (var itemData in shopItems)
        {
            itemList.Add(new Inventory_Item(itemData));
        }
    }
    public void SetInventory(Inventory_Player inventory) => playerInventory = inventory;


    // �������Ʒ
    public void BuyItem(Inventory_Item item)
    {
        BuyableItemDataSo buyableData = item.itemData as BuyableItemDataSo;
        if (buyableData == null) return;

        if (playerInventory.CanAddItem(item) == false)
            return;

        if (!playerInventory.CanUseGold(buyableData.price))//�������������£�//�ж�Ǯ������
                                                           // ����� �� ��Ǯ,������return
            return;

        RemoveItem(item);
        playerInventory.AddItem(item);

        InvokeInventoryChange();
    }
}