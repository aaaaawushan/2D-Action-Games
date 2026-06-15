using Unity.VisualScripting;
using UnityEngine;
using System.Collections;

public class Enemy : Entity
{
    public Enemy_IdleState idleState;
    public Enemy_MoveState moveState;
    public Enemy_AttackState attackState;
    public Enemy_BattleState battleState;
    public Enemy_DeadState deadState;
    public Enemy_StunnedState stunnedState;

    [Header("Movement details")]
    public float idleTime = 1f;
    public float moveSpeed = 1.4f;
    [Range(0, 2)]
    public float moveAnimSpeedMultiplier = 1;

    [Header("player Detection ")]
    public LayerMask WhatIsPlayer;
    public float PlayerCheckDistance = 10;
    [SerializeField] private Transform PlayerCheck;
    public Transform player { get; protected set; }

    [Header("Battle details")]
    public float battleMoveSpeed = 5;
    public float attackDistance = 2;
    public float battleTimerDuration = 3;
    public float minRereatDistance = 1;
    public Vector2 retreatVelocity;


    [Header("stunned details")]
    [SerializeField] public float stunnedDuration = 1f;
    [SerializeField] public Vector2 stunnedVelocity = new Vector2(7f, 7f);
    [SerializeField] protected bool canBeStunned;
    protected override void Awake()
    {
        base.Awake();
    }
    public void EnableCounterWindow(bool enable) => canBeStunned = enable;

    public static event System.Action OnAnyEnemyDeath;
    public override void EntityDeath()
    {
        base.EntityDeath();
        stateMachine.ChangeState(deadState);
        OnAnyEnemyDeath?.Invoke();
    }
    public virtual void TryEnterAttackState(Transform player)
    {
        if (stateMachine.currentState == battleState || stateMachine.currentState == attackState) return;
        this.player = player;
        stateMachine.ChangeState(battleState);
    }

    public RaycastHit2D PlayerDetection()
    {
        RaycastHit2D hit = Physics2D.Raycast(PlayerCheck.position, Vector2.right * facedir, PlayerCheckDistance, WhatIsPlayer);
        if (hit.collider != null && hit.collider.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            return hit;
        }

        hit = Physics2D.Raycast(PlayerCheck.position, Vector2.right * -facedir, PlayerCheckDistance, WhatIsPlayer);
        if (hit.collider != null && hit.collider.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            return hit;
        }
        return default;
    }

    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(PlayerCheck.position, new Vector3(PlayerCheck.position.x + (facedir * PlayerCheckDistance), PlayerCheck.position.y));
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(PlayerCheck.position, new Vector3(PlayerCheck.position.x + (facedir * attackDistance), PlayerCheck.position.y));
        Gizmos.color = Color.green;
        Gizmos.DrawLine(PlayerCheck.position, new Vector3(PlayerCheck.position.x + (facedir * minRereatDistance), PlayerCheck.position.y));
    }
    protected virtual void HandlePlayerDeath()
    {
        if (idleState != null)
            stateMachine.ChangeState(idleState);
    }
    private void OnEnable()
    {
        Player.OnPlayDeath += HandlePlayerDeath;
    }
    private void OnDisable()
    {
        Player.OnPlayDeath -= HandlePlayerDeath;
    }
    protected override IEnumerator SlowDownEntityCo(float duration, float slowMultiplier)
    {
        float originalMoveSpeed = moveSpeed;
        float originalBattleSpeed = battleMoveSpeed;
        float originalAnimSpeed = anim.speed;

        float speedMultiplier = 1 - slowMultiplier;

        moveSpeed *= speedMultiplier;
        battleMoveSpeed *= speedMultiplier;
        anim.speed *= speedMultiplier;

        yield return new WaitForSeconds(duration);

        moveSpeed = originalMoveSpeed;
        battleMoveSpeed = originalBattleSpeed;
        anim.speed = originalAnimSpeed;
    }
}
