using UnityEngine;

public class Object_GoldPickup : Object_ItemPickup
{
    [SerializeField] private Vector2 dropForce = new Vector2(3, 10);
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Collider2D col;

    private int goldAmount;
    public void SetupGold(int amount)
    {
        goldAmount = amount;

        float xForce = Random.Range(-dropForce.x, dropForce.x);
        rb.linearVelocity = new Vector2(xForce, dropForce.y);
        col.isTrigger = false;

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Ground")
            && col.isTrigger == false)
        {
            col.isTrigger = true;
            rb.constraints = RigidbodyConstraints2D.FreezeAll;
        }
        Inventory_Player inventory = collision.gameObject.GetComponent<Inventory_Player>();
        if (inventory != null)
        {
            inventory.AddGold(goldAmount);
            Destroy(gameObject);
        }
    }
    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        Inventory_Player inventory = collision.GetComponent<Inventory_Player>();
        if (inventory == null) return;

        inventory.AddGold(goldAmount);
        Destroy(gameObject);
    }
}