using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.HID;

public class Enemy_ShadowAttack2State : Enemy_ShadowState
{
    private GameObject[] spikes;
    private Transform[] selectedPos;


    public Enemy_ShadowAttack2State(Enemy_Shadow shadowEnemy, StateMachine stateMachine, string stateName) : base(shadowEnemy, stateMachine, stateName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        rb.linearVelocity = Vector2.zero;
        shadowEnemy.attackFinished = false;
        shadowEnemy.cooldownTime = 0.1f;

        spikes = null;

        if (shadowEnemy.isPhase2)
        {
            selectedPos = PickRandomPos();
            shadowEnemy.selectedPoints = selectedPos;
            shadowEnemy.SpawnSpikeWarning(selectedPos);
        }
    }

    public override void Update()
    {
        base.Update();

        if (shadowEnemy.attackFinished) shadowEnemy.cooldownTime -= Time.deltaTime;
        if (shadowEnemy.cooldownTime <= 0) stateMachine.ChangeState(shadowEnemy.shadowChaseState);
    }

    // 動画イベント
    public void OnSpikesSpawned(GameObject[] newSpikes)
    {
        spikes = newSpikes;
    }

    public override void CallAnimationTrigger()
    {
        shadowEnemy.attackFinished = true;
        shadowEnemy.SetVelocity(0, 0);
    }

    public override void Exit()
    {
        base.Exit();
        if (spikes != null)
        {
            foreach (var spike in spikes)
            {
                if (spike != null)
                {
                    spike.GetComponent<Object_BossSpike>().SpikeHide();
                }
            }
            spikes = null;
        }
    }

    private Transform[] PickRandomPos()
    {
        List<int> indices = new List<int>();
        for (int i = 0; i < shadowEnemy.spikeSpawnPoints.Length; i++)
            indices.Add(i);

        for (int i = indices.Count - 1; i > 0; i--)
        {
            int j = Random.Range(0, i + 1);
            int temp = indices[i];
            indices[i] = indices[j];
            indices[j] = temp;
        }

        int count = Mathf.Min(shadowEnemy.spikeCount, indices.Count);
        Transform[] result = new Transform[count];
        for (int i = 0; i < count; i++)
        {
            result[i] = shadowEnemy.spikeSpawnPoints[indices[i]];
        }
        return result;
    }
}