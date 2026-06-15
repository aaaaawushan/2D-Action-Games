using UnityEngine;

public class Object_NPC : MonoBehaviour
{
    protected Transform player;
    protected UI ui;

    [SerializeField] private Transform npc;
    [SerializeField] private GameObject ToolTip;
    private bool facingRight = true;

    [Header("float tooltip")]
    [SerializeField] private float  floatSpeed = 6f;
    [SerializeField] private float floatRange = .2f;
    private Vector3 startPosition;

    protected virtual void Awake()
    {
        ui = FindFirstObjectByType<UI>();
        startPosition = ToolTip.transform.position;
        ToolTip.SetActive(false);
    }

    protected virtual void Update()
    {
        HandleNpcFlip();
        HandleToolTipFloat();
    }
    private void HandleNpcFlip()
    {
        if (player == null || npc == null)
            return;

        if (npc.position.x > player.position.x && facingRight == true)
        {
            npc.transform.Rotate(0, 180,0);
            facingRight = false;
        }else if (npc.position.x < player.position.x && facingRight == false)
        {
            npc.transform.Rotate(0, 180, 0);
            facingRight = true;
        }

    }
    protected void HandleToolTipFloat()
    {
        if (ToolTip.activeSelf)
        {
            float offset = Mathf.Sin(floatSpeed * Time.time) * floatRange;
            ToolTip.transform.position = startPosition + new Vector3(0, offset);
        }
    }
    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Player>() == null) return;
        player = collision.transform;
        ToolTip.SetActive(true);
    }

    protected virtual void OnTriggerExit2D(Collider2D collision)
    {
        if (ToolTip != null)
            ToolTip.SetActive(false);
    }
}
