using UnityEngine;

public class Enemy_ShadowPhaseTransitionState : Enemy_ShadowState
{
    public Enemy_ShadowPhaseTransitionState(Enemy_Shadow shadowEnemy, StateMachine stateMachine, string stateName) : base(shadowEnemy, stateMachine, stateName)
    {
    }
    private float timer;

    public override void Enter()
    {
        base.Enter();
        enemy.rb.linearVelocity = new Vector2(enemy.rb.linearVelocity.x, 0f);
        timer = 2f;

        shadowEnemy.isInvulnerable = true;
        shadowEnemy.anim.SetBool("PhaseTransition", true);
        shadowEnemy.phaseTransitionParticles.Play();

        AudioSource.PlayClipAtPoint(shadowEnemy.phaseTransitionSFX, shadowEnemy.transform.position);
    }

    public override void Update()
    {
        base.Update();
        timer -= Time.deltaTime;

        if (timer <= 0f)
        {
            shadowEnemy.SwitchToPhase2();
            stateMachine.ChangeState(shadowEnemy.shadowChaseState);
        }
    }

    public override void Exit()
    {
        base.Exit();
        shadowEnemy.isInvulnerable = false;
        shadowEnemy.anim.SetBool("PhaseTransition", false);
    }
}
