using TMPro;
using UnityEngine;

public class UI_Gold : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI goldAmout;
    [SerializeField] private Inventory_Player playerInventory;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GoldAmountChange();
        playerInventory.OnGoldChange += GoldAmountChange;
        //  playerInventory = FindAnyObjectByType<Inventory_Player>();
    }

    // Update is called once per frame
    private void GoldAmountChange()
    {
        goldAmout.text = playerInventory.gold.ToString();

    }
}
