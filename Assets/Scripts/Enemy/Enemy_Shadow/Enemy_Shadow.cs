using System.Collections;
using UnityEngine;

public class Enemy_Shadow : Enemy
{
    public Enemy_ShadowChaseState shadowChaseState;
    public Enemy_ShadowIdleState shadowIdleState;
    public Enemy_ShadowAttack1State shadowAttack1State;
    public Enemy_ShadowAttack2State shadowAttack2State;
    public Enemy_ShadowAttack3State shadowAttack3State;
    public Enemy_ShadowDeadState shadowDeadState;
    public Enemy_ShadowPhaseTransitionState shadowPhaseTransitionState;


    [Header("Battle Details")]
    public bool battleStarted = false;
    public bool attackFinished = false;
    public float cooldownTime;
    private int attackIndex = 0;
    private int[] phase1Pattern = { 1, 2, 1, 3, 2, 1 };
    private int[] phase2Pattern = { 3, 1, 2, 3, 2, 1 };

    [Header("Phase Transition")]
    public ParticleSystem phaseTransitionParticles;
    public AudioClip phaseTransitionSFX;
    public bool isInvulnerable;

    [Header("Attack Check Points")]
    public Transform attack1Check;
    public Transform attack2Check;
    public Transform attack3Check;
    public float attack1Radius;
    public float attack2Radius;
    public float attack3Radius;
    [SerializeField] private float phase2BattleSpeed = 3.5f;

    [Header("Attack2 SpikeVFX")]
    public GameObject spikePrefab;
    public Transform[] spikeSpawnPoints;
    public ParticleSystem attack2SpikeParticles;
    public int spikeCount = 4;
    public bool isPhase2 = false;
    [HideInInspector] public Transform[] selectedPoints;
    [SerializeField] private AudioClip spikeSe;
  //  [SerializeField] private AudioSource audioSource;

    [Header("Attack3 Roll")]
    public float attack3RollSpeed = 5f;
    private bool isAttack3Active;

    [Header("Movement Bounds")]
    public Transform leftBound;
    public Transform rightBound;

    protected override void Awake()
    {
        base.Awake();
        shadowIdleState = new Enemy_ShadowIdleState(this, stateMachine, "idle");
        shadowChaseState = new Enemy_ShadowChaseState(this, stateMachine, "chase");
        shadowAttack1State = new Enemy_ShadowAttack1State(this, stateMachine, "attack1");
        shadowAttack2State = new Enemy_ShadowAttack2State(this, stateMachine, "attack2");
        shadowAttack3State = new Enemy_ShadowAttack3State(this, stateMachine, "attack3");
        shadowDeadState = new Enemy_ShadowDeadState(this, stateMachine, "dead");
        shadowPhaseTransitionState = new Enemy_ShadowPhaseTransitionState(this, stateMachine, "PhaseTransition");
    }
    protected override void Start()
    {
        base.Start();
        stateMachine.Initialize(shadowIdleState);
       
    }

    public void SpawnSpikeWarning(Transform[] points)
    {
        foreach (var point in points)
        {
            Instantiate(attack2SpikeParticles, point.position, attack2SpikeParticles.transform.rotation);

            AudioSource audio = GetComponent<Enemy_ShadowHealth>().enemyAudio;
            if (audio != null && spikeSe != null)
            {
                audio.PlayOneShot(spikeSe, 0.5f);
            }
        }
    }
    public void SpawnSpikes()
    {
        if (!isPhase2 || selectedPoints == null) return;

        GameObject[] spikes = new GameObject[selectedPoints.Length];
        for (int i = 0; i < selectedPoints.Length; i++)
        {
            spikes[i] = Instantiate(spikePrefab, selectedPoints[i].position, Quaternion.identity);
        }

        if (stateMachine.currentState is Enemy_ShadowAttack2State attack2)
        {
            attack2.OnSpikesSpawned(spikes);
        }
    }

    public void KeepWithinBounds()
    {
        float boundPos = Mathf.Clamp(transform.position.x, leftBound.position.x, rightBound.position.x);
        transform.position = new Vector2(boundPos, transform.position.y);

    }
    public void SwitchToPhase2()
    {
        phase1Pattern = phase2Pattern;
        attackIndex = 0;
        battleMoveSpeed = phase2BattleSpeed;

        var health = GetComponent<Enemy_ShadowHealth>();
        health.StartRegen();
        health.ChangeHealthBarColor(new Color(0.7f, 0f, 0f));
    }
    public void EnterPhaseTransition()
    {
        Debug.Log("boss pahse2");
        stateMachine.ChangeState(shadowPhaseTransitionState);
    }
    public void SetAttack3Active(bool active) => isAttack3Active = active;
    private void LateUpdate()
    {
        if (isAttack3Active)
            anim.transform.localPosition = Vector3.zero;
    }
    public void PerformBossAttack(Transform checkPoint, float radius)
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(checkPoint.position, radius, WhatIsPlayer);
        Debug.Log("Hits: " + hits.Length);
        foreach (var hit in hits)
        {
            IDamageable damageable = hit.GetComponent<IDamageable>();
            Debug.Log("Damageable: " + (damageable != null));
            damageable.TakeDamage(stats.GetPhysicalDamage(out bool isCrit, 1.5f), 0, ElementType.None, transform);

        }
    }
    public override void TryEnterAttackState(Transform player)
    {

    }
    public override void EntityDeath()
    {
        stateMachine.ChangeState(shadowDeadState);
    }
    public void RandomAttackState()
    {
        int attack = phase1Pattern[attackIndex];
        attackIndex = (attackIndex + 1) % phase1Pattern.Length;
        switch (attack)
        {
            case 1: stateMachine.ChangeState(shadowAttack1State); break;
            case 2: stateMachine.ChangeState(shadowAttack2State); break;
            case 3: stateMachine.ChangeState(shadowAttack3State); break;
        }

    }
    public void StartBattle()
    {
        battleStarted = true;
        player = FindAnyObjectByType<Player>().transform;
        float dir = Mathf.Sign(player.position.x - transform.position.x);
        if (dir != facedir) Flip();
        stateMachine.ChangeState(shadowChaseState);
    }
    public void SetPlayer(Transform target)
    {
        player = target;
    }
    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();

        Gizmos.color = Color.red;
        if (attack1Check != null) Gizmos.DrawWireSphere(attack1Check.position, attack1Radius);

        Gizmos.color = Color.blue;
        if (attack2Check != null) Gizmos.DrawWireSphere(attack2Check.position, attack2Radius);

        Gizmos.color = Color.yellow;
        if (attack3Check != null) Gizmos.DrawWireSphere(attack3Check.position, attack3Radius);
    }

}
