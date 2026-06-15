using UnityEngine;

public class VFX_SpellWave : MonoBehaviour
{
    [SerializeField] private float speed = 15f;
    [SerializeField] private float damage = 3f;
    [SerializeField] private float lifetime = 0.5f;

    private int direction;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void Setup(int dir)
    {
        direction = dir;

        if (dir < 0)
            transform.Rotate(0, 180, 0);

        Destroy(gameObject, lifetime);
    }

    void Update()
    {
        transform.position += Vector3.right * direction * speed * Time.deltaTime;

    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player")) return;
        IDamageable target = collision.GetComponent<IDamageable>();
        if (target != null)
        {
            target.TakeDamage(damage, 0, ElementType.None, transform);
        }
    }
}
