using UnityEngine;

public class Enemy_AnimationTrigger : Entity_AnimationTrigger
{
    private Enemy enemy;

    protected override void Awake()
    {
        base.Awake();
        enemy = GetComponentInParent<Enemy>();
    }
    private void EnableCounterWindow()
    {
        enemy.EnableCounterWindow(true);
    }
    private void DisableCounterWindow()
    {
        enemy.EnableCounterWindow(false);
    }
    private void BossAttack1Trigger()
    {
        var boss = GetComponentInParent<Enemy_Shadow>();
        boss.PerformBossAttack(boss.attack1Check, boss.attack1Radius);
    }

    private void BossAttack2Trigger()
    {
        var boss = GetComponentInParent<Enemy_Shadow>();
        boss.PerformBossAttack(boss.attack2Check, boss.attack2Radius);
    }

    private void BossAttack3Trigger()
    {
        var boss = GetComponentInParent<Enemy_Shadow>();
        boss.PerformBossAttack(boss.attack3Check, boss.attack3Radius);
    }
    //ボス戦stage2のattack2時のスパイク
    public void SpikeTrigger()
    {
        Enemy_Shadow shadow = GetComponentInParent<Enemy_Shadow>();
        if (shadow != null && shadow.isPhase2)
        {
            shadow.SpawnSpikes();
        }
    }
}

