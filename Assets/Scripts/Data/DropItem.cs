using UnityEngine;

[System.Serializable]
public class DropItem
{
    public ItemDataSO itemData;
    [Range(0, 1)]
    public float dropChance = 0.5f;
    public int minAmount = 1;   
    public int maxAmount = 1;  
}