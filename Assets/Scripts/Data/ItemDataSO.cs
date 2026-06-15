using UnityEngine;

[CreateAssetMenu(menuName = "RPG Setup/Item Data/Material item")]
public class ItemDataSO : ScriptableObject
                                          
{
    public string itemName;
    public Sprite itemIcon;
    public ItemType itemType;
    public int maxStackSize=5;

    [Header("Item effect")]
    public ItemEffect_DataSO itemEffect;
}