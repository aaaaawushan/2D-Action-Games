using UnityEngine;

public class Object_ItemPickup : MonoBehaviour
{
    private SpriteRenderer sr;

    [SerializeField] private ItemDataSO itemData;

    private Inventory_Item itemToAdd;
    private Inventory_Player playerInventory;

    private void Awake()
    {
        if (itemData != null)                         
            itemToAdd = new Inventory_Item(itemData);
    }

    private void OnValidate()//让编辑器不启动游戏就可以改变
    {
        if (itemData == null)
            return;

        sr = GetComponent<SpriteRenderer>();
        sr.sprite = itemData.itemIcon;
        gameObject.name = "Object_ItemPickup - " + itemData.itemName;
    }
    public void SetItemData(ItemDataSO data)
    {
        itemData = data;
        itemToAdd = new Inventory_Item(data);

        sr = GetComponent<SpriteRenderer>();
        sr.sprite = data.itemIcon;  // 设置正确的图标
    }

    protected virtual void OnTriggerEnter2D(Collider2D collision)//检测是不是能捡的东西
    {
        playerInventory = collision.GetComponent<Inventory_Player>();
        if (playerInventory == null) return;

        if (itemData.itemType == ItemType.Gold)
        {
            playerInventory.AddGold(1);
            Destroy(gameObject);
            return;
        }

        if (playerInventory.CanAddItem(itemToAdd))
        {
            playerInventory.AddItem(itemToAdd);
            Destroy(gameObject);
        }
    }
}