using System.Collections;
using UnityEngine;
using System;

public class Entity : MonoBehaviour
{
    public event Action OnFlipped;
    protected StateMachine stateMachine;
    public Animator anim { get; private set; }

    public Rigidbody2D rb { get; private set; }
    public Entity_Stats stats { get; private set; }
    private bool facingRight = true;
    public int facedir { get; private set; } = 1;

    [Header("Collision detection")]
    [SerializeField] protected float groundCheckDistance;
    [SerializeField] protected float wallCheckDistance;
    [SerializeField] protected LayerMask GroundMask;
    public LayerMask WallMask;
    [SerializeField] protected Transform groundCheck;

    [SerializeField] public bool Isgrounded;
    [SerializeField] public bool Iswall;

    private bool isKnocked;
    private Coroutine knockCoroutine;
    private Coroutine slowDownCo;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected virtual void Awake()
    {
        anim = GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody2D>();
        stats = GetComponent<Entity_Stats>();

        stateMachine = new StateMachine();

    }

    protected virtual void Start()
    {
    }

    protected virtual void Update()
    {
        HanCollisionDetection();
        stateMachine.UpdateActiveState();

    }
    public virtual void EntityDeath()
    {

    }
    public void ReceiveKnockback(Vector2 knockback, float duration)
    {
        if (knockCoroutine != null) StopCoroutine(knockCoroutine);
        knockCoroutine = StartCoroutine(KnockBackCo(knockback, duration));

    }
    private IEnumerator KnockBackCo(Vector2 knockback, float duration)
    {
        isKnocked = true;
        rb.linearVelocity = knockback;

        //knockbackの時プレイヤーのinputが無効化にする
        Player player = GetComponent<Player>();
        // if (player != null)がプレイヤーだけinput無効化するための確保
        if (player != null) player.input.Disable();

        yield return new WaitForSeconds(duration);

        rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);

        isKnocked = false;
        if (player != null) player.input.Enable();
    
    }
    public void CurrentStateAnimationTrigger()
    {
        stateMachine.currentState.CallAnimationTrigger();
    }

    public void SetVelocity(float xVelocity, float yVelocity)
    {
        if (isKnocked) return;
        rb.linearVelocity = new Vector2(xVelocity, yVelocity);
        HandleFlip(xVelocity);
    }
    public void HandleFlip(float xVelocity)
    {
        if (((xVelocity > 0) && !facingRight))
        {
            Flip();
        }
        else if ((xVelocity < 0) && facingRight)
        {
            Flip();
        }
    }
    public void Flip()
    {
        transform.Rotate(0, 180, 0);
        facingRight = !facingRight;
        facedir = facedir * -1;


        OnFlipped?.Invoke();
    }
    protected virtual void HanCollisionDetection()
    {
        Isgrounded = Physics2D.Raycast(groundCheck.position, Vector2.down, groundCheckDistance, GroundMask);
        Iswall = Physics2D.Raycast(transform.position, Vector2.right * facedir, wallCheckDistance, WallMask);
    }
    protected virtual void OnDrawGizmos()
    {
        Gizmos.DrawLine(groundCheck.position, groundCheck.position - new Vector3(0, groundCheckDistance, 0));
        Gizmos.DrawLine(transform.position, transform.position + new Vector3(wallCheckDistance * facedir, 0, 0));
    }
    public virtual void SlowDownEntity(float duration, float slowMultiplier)
    {
        if (slowDownCo != null)
            StopCoroutine(slowDownCo);

        slowDownCo =
            StartCoroutine(SlowDownEntityCo(duration, slowMultiplier));
    }

    protected virtual IEnumerator SlowDownEntityCo(float duration, float slowMultiplier)
    {
        yield return null;
    }
}
