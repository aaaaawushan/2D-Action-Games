using UnityEngine;

public class Object_Spike : MonoBehaviour
{
    [SerializeField] private GameObject DeadPanel;
    [SerializeField] private float spikeDamage = 3f;
    protected Player player;

    protected virtual void Start()
    {
        player = FindAnyObjectByType<Player>();
        if (DeadPanel != null)
            DeadPanel.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && collision.GetComponent<Player>() != null)
        {
            player.health.TakeDamage(spikeDamage, 0, ElementType.None, transform);

            if (player.health.IsDead && !DeadPanel.activeSelf)
            {
                DeadPanel.SetActive(true);
            }
        }
    }
}