using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Object_Chest : MonoBehaviour,IDamageable
{
    private Animator anim => GetComponentInChildren<Animator>();
    [Header("Open details")]
    [SerializeField] public Vector2 knockback;

    [SerializeField] private AudioClip chestOpen;
    private Entity_VFX fx => GetComponent<Entity_VFX>();

    [Header("Item Drop")]
    private Enemy_GoldDrop goldDrop;
  //  [SerializeField] private Transform itemDropPos;
    [SerializeField] private GameObject itemPickupPrefab;
    [SerializeField] private DropItem[] possibleDrops;

    private void Awake()
    {
        goldDrop =GetComponent<Enemy_GoldDrop>();
    }

    private void SpawnDrops()
    {
        foreach (var drop in possibleDrops)
        {
            if (Random.value <= drop.dropChance)
            {
                int amount = Random.Range(drop.minAmount, drop.maxAmount + 1);
                for (int i = 0; i < amount; i++)
                {
                    GameObject obj = Instantiate(itemPickupPrefab,transform.position, Quaternion.identity);
                    Object_ItemPickup pickup = obj.GetComponent<Object_ItemPickup>();
                    pickup.SetItemData(drop.itemData);
                }
            }
        }
    }
    public bool TakeDamage(float damage,float elementalDamage, ElementType element, Transform DamageDealer)
    {
        fx.PlayOnDamageVfx();
        anim.SetBool("chestOpen", true);
        AudioSource.PlayClipAtPoint(chestOpen, transform.position);
        goldDrop.DropGold();
     
        SpawnDrops();
        foreach (var col in GetComponentsInChildren<Collider2D>())
            col.enabled = false;
        return true;
    }

}
