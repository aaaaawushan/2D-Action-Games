using System;
using System.Collections;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;

public class Player : Entity//所有在player里创建状态切换和符合状态蓝本的每个状态的实例
                            //玩家的"大脑/数据中心",持有所有数据（速度、跳力、输入等）创建所有状态的实例,管理组件引用
{
    private UI ui;
    public PlayerInput input { get; private set; }
    public Player_SkillManager skillManager;
    public Vector2 moveInput { get; private set; }
    public AudioSource seSource;

    [Header("Jump Feel")]
    public float coyoteTime = 0.2f;
    public float jumpBufferTime = 0.1f;
    public float coyoteTimeCounter { get; set; }
    public float jumpBufferCounter { get; set; }
    [SerializeField] private float groundCheckWidth = 0.3f;

    [Header("health details")]
    public GameObject retryButton;
    public static event Action OnPlayDeath;
    public Player_Health health;

    [Header("heal details")]
    public float healAmount = 5f;
    public Slider manaBar;
    public float maxMana = 20f;
    public float currentMana;
    public float healManaCost = 10f;
    public ParticleSystem psAttract;
    public ParticleSystem psBurst;
    public AudioClip healClip;
    public bool justHealed { get; set; }


    [Header("Movement details")]
    public float moveSpeed;
    public Vector2 wallJumpForce;
    public float jumpForce = 5;
    public float fallMultiplier = 3f;
    [Range(0, 1)]
    public float inAirMoveMultiplier = .7f;
    [Range(0, 1)]
    public float wallSliderSlowMultiplier = 0.7f;
    [Space]
    public float dashDuration = .25f;
    public float dashSpeed = 20;


    [Header("Attack details")]
    public Vector2[] attackVelocity;//这里是一个集合包含着三个攻击模式，不同的攻击有着不同的攻击速度
    public float attackDuration = .1f;
    public float comboResetTime = 1f;
    private Coroutine queuedAttackCo;

    [Header("Spell details")]
    [SerializeField] private GameObject spellPrefab;
    [SerializeField] private Transform spellSpawnPoint;
    [SerializeField] private float spellCooldown = 1f;
    [SerializeField] private float recoilForce = 3f;
    [SerializeField] private AudioClip spellClip;
    private bool canCast = true;
    private float castTimer;


    #region State Variables
    public Player_IdleState idleState { get; private set; }//这些状态们都是因为entity没有mono所以要创建他们的实例
    public Player_MoveState moveState { get; private set; }

    public Player_JumpState jumpState { get; private set; }
    public Player_Fallstate fallState { get; private set; }
    public Player_WallSliedState wallSliedState { get; private set; }
    public Player_WallJumpState wallJumpState { get; private set; }
    public Player_DashState dashState { get; private set; }
    public Player_BasicAttackState basicAttackState { get; private set; }
    public Player_JumpAttackState jumpAttackState { get; private set; }
    public Player_DeadState deadState { get; private set; }
    public Player_VFX vfx { get; private set; }
    public Player_HealState healState { get; private set; }
    #endregion
    protected override void Awake()
    {
        base.Awake();
        ui = FindAnyObjectByType<UI>();
        input = new PlayerInput();//普通 C# 类用new去创建新的东西
        skillManager = GetComponent<Player_SkillManager>();
        vfx = GetComponent<Player_VFX>();
        health = GetComponent<Player_Health>();

        idleState = new Player_IdleState(this, stateMachine, "idle");
        moveState = new Player_MoveState(this, stateMachine, "move");
        jumpState = new Player_JumpState(this, stateMachine, "jumpFall");
        fallState = new Player_Fallstate(this, stateMachine, "jumpFall");
        wallSliedState = new Player_WallSliedState(this, stateMachine, "wallSlied");
        wallJumpState = new Player_WallJumpState(this, stateMachine, "jumpFall");
        dashState = new Player_DashState(this, stateMachine, "dash");
        basicAttackState = new Player_BasicAttackState(this, stateMachine, "basicAttack");
        jumpAttackState = new Player_JumpAttackState(this, stateMachine, "jumpAttack");
        deadState = new Player_DeadState(this, stateMachine, "dead");
        healState = new Player_HealState(this, stateMachine, "heal");

    }
    protected override void Start()
    {
        base.Start();
        stateMachine.Initialize(idleState);
        currentMana = maxMana;
        health.UpdateManaBar();
    }
    protected override void Update()
    {
        base.Update();
        health.ResetDamageFrame();

        // 地面にいる間はコヨーテタイムをリセット、空中では減らす
        if (Isgrounded)
            coyoteTimeCounter = coyoteTime;
        else
            coyoteTimeCounter -= Time.deltaTime;

        // ジャンプボタンを押したらバッファをセット、押してない間は減らす
        if (input.Player.Jump.WasPerformedThisFrame())
            jumpBufferCounter = jumpBufferTime;
        else
            jumpBufferCounter -= Time.deltaTime;
        //押し続かないと回復できない
        if (input.Player.Heal.IsPressed())
            TryEnterHealState();

        if (input.Player.Heal.WasReleasedThisFrame())
            justHealed = false;

        if (!canCast)
        {
            castTimer -= Time.deltaTime;
            if (castTimer <= 0) canCast = true;
        }

        if (rb.linearVelocity.y < 0)
        {
            rb.linearVelocity += Vector2.up * Physics2D.gravity.y * fallMultiplier * Time.deltaTime;
        }
    }

    protected override void HanCollisionDetection()
    {
        bool centerCheck = Physics2D.Raycast(groundCheck.position, Vector2.down, groundCheckDistance, GroundMask);
        bool leftCheck = Physics2D.Raycast(groundCheck.position - new Vector3(-groundCheckWidth, 0, 0), Vector2.down, groundCheckDistance, GroundMask);
        bool rightCheck = Physics2D.Raycast(groundCheck.position + new Vector3(-groundCheckWidth, 0, 0), Vector2.down, groundCheckDistance, GroundMask);
        Isgrounded = centerCheck || leftCheck || rightCheck;
        Iswall = Physics2D.Raycast(transform.position, Vector2.right * facedir, wallCheckDistance, WallMask);
    }

    protected override void OnDrawGizmos()
    {
        Vector3 leftStart = groundCheck.position + new Vector3(-groundCheckWidth, 0, 0);
        Vector3 rightStart = groundCheck.position + new Vector3(groundCheckWidth, 0, 0);

        Gizmos.DrawLine(groundCheck.position, groundCheck.position + Vector3.down * groundCheckDistance);
        Gizmos.DrawLine(leftStart, leftStart + Vector3.down * groundCheckDistance);
        Gizmos.DrawLine(rightStart, rightStart + Vector3.down * groundCheckDistance);

        Gizmos.DrawLine(transform.position, transform.position + new Vector3(wallCheckDistance * facedir, 0, 0));
    }


    public bool CanHeal => Isgrounded && health.currentHP < health.entityStats.GetMaxHealth() && currentMana >= healManaCost;

    public void TryEnterHealState()
    {
        if (justHealed) return;
        if (stateMachine.currentState == healState) return;
        if (CanHeal) stateMachine.ChangeState(healState);

    }
    //回復のスキルを実行する
    public bool ApplyHeal()
    {
        if (currentMana < healManaCost) return false;
        currentMana -= healManaCost;
        currentMana = Mathf.Max(currentMana, 0);
        health.IncreaseHealth(healAmount);
        health.UpdateManaBar();
        return true;
    }
    //魔法エフェクト
    public void CastSpell()
    {
        if (!canCast) return;
        var spell = Instantiate(spellPrefab, spellSpawnPoint.position, Quaternion.identity);
        spell.GetComponent<VFX_SpellWave>().Setup(facedir);

        rb.AddForce(new Vector2(-facedir * recoilForce, 0f), ForceMode2D.Impulse);
        seSource.PlayOneShot(spellClip);
        canCast = false;
        castTimer = spellCooldown;
    }
    private void CutJump()
    {
        if (rb.linearVelocity.y > 0)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, rb.linearVelocity.y * 0.5f);
        }
    }
    public void FreezePlayer(bool freeze)
    {
        if (freeze)
        {
            stateMachine.ChangeState(idleState);
            rb.linearVelocity = Vector2.zero;
            rb.gravityScale = 0;
            anim.speed = 0;
        }
        else
        {
            rb.gravityScale = 4;
            anim.speed = 1;
        }
        enabled = !freeze;
    }
    public void TeleportPlayer(Vector3 position) => transform.position = position;// 直接把玩家传送到指定位置
    public bool IsWallSliding() => stateMachine.currentState == wallSliedState;
    private void TryInteract()
    {
        IInteractable closest = null;
        float closestDistance = Mathf.Infinity;
        Collider2D[] objectsAround = Physics2D.OverlapCircleAll(transform.position, 1.5f);

        foreach (var target in objectsAround)
        {
            IInteractable interactable = target.GetComponent<IInteractable>();
            if (interactable == null) continue;

            float distance = Vector2.Distance(transform.position, target.transform.position);

            if (distance < closestDistance)
            {
                closestDistance = distance;
                closest = interactable; 
            }
        }

        if (closest == null)
            return;

        closest.Interact();
    }

    public override void EntityDeath()
    {
        base.EntityDeath();
        stateMachine.ChangeState(deadState);
        OnPlayDeath?.Invoke();
    }
    private void OnEnable()
    {
        input.Enable();
        input.Player.movement.performed += ctx => moveInput = ctx.ReadValue<Vector2>();
        input.Player.movement.canceled += ctx => moveInput = Vector2.zero;
        input.Player.Jump.canceled += ctx => CutJump();
        input.Player.Spell.performed += ctx => CastSpell();

        //input.Player.ToggleSkillTreeUI.performed += ctx => ui.ToggleSkillTreeUI();
        input.Player.ToggleInventoyUI.performed += ctx => ui.ToggleInventoryUI();
        input.Player.Interact.performed += ctx => TryInteract();
    }
    private void OnDisable()
    {
        input.Disable();
    }
    public void EnterAttackStateWithDelay()// 延迟一帧再进入攻击状态
                                           // 用于连招：当前攻击动画还没结束时，
                                           // 先排队等待，下一帧再切换
    {
        if (queuedAttackCo != null)
        {
            StopCoroutine(queuedAttackCo);
        }
        queuedAttackCo = StartCoroutine(EnterAttackStateWithDelayCo());
    }
    private IEnumerator EnterAttackStateWithDelayCo()
    {
        yield return new WaitForEndOfFrame();
        stateMachine.ChangeState(basicAttackState);

    }
    protected override IEnumerator SlowDownEntityCo(float duration, float slowMultiplier)// 减速效果的协程
                                                                                         // 记录原始速度 → 按比例降低 → 等待duration秒 → 恢复原始速度
                                                                                         // 影响：移动速度、跳力、动画速度、蹬墙跳力
    {
        float originalMoveSpeed = moveSpeed;
        float originalJumpForce = jumpForce;
        float originalAnimSpeed = anim.speed;

        Vector2 originalWallJump = wallJumpForce;

        float speedMultiplier = 1 - slowMultiplier;

        moveSpeed *= speedMultiplier;
        jumpForce *= speedMultiplier;
        anim.speed *= speedMultiplier;
        wallJumpForce *= speedMultiplier;


        yield return new WaitForSeconds(duration);

        moveSpeed = originalMoveSpeed;
        jumpForce = originalJumpForce;
        anim.speed = originalAnimSpeed;
        wallJumpForce = originalWallJump;


    }

}
