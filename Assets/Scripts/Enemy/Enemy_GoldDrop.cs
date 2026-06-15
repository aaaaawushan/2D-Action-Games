using UnityEngine;

public class Enemy_GoldDrop : MonoBehaviour
{
    [SerializeField] private GameObject goldDropPrefab;
    [SerializeField] private int minGoldDrop = 1;
    [SerializeField] private int maxGoldDrop = 8;

    public void DropGold()
    {
        int goldAmount = Random.Range(minGoldDrop, maxGoldDrop + 1);

        for (int i = 0; i < goldAmount; i++)
        {
            GameObject newGold = Instantiate(goldDropPrefab, transform.position, Quaternion.identity);
            Object_GoldPickup goldPickup = newGold.GetComponent<Object_GoldPickup>();//Èç¹û²»ÕâÑùĞ´£¬Ö±½Ó newGold.SetupGold(1) ÊÇ²»ĞĞµÄ
                                                                                     //ÒòÎª newGold ÊÇGameObjectÀàĞÍ
                                                                                     //ËE»ÖªµÀ×Ô¼ºÉúåÏ¹ÒÁËÊ²Ã´½Å±¾
            if (goldPickup != null)
                goldPickup.SetupGold(1);
        }
    }
}