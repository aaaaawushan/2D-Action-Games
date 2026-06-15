using Unity.Cinemachine;
using UnityEngine;
using static UnityEngine.ParticleSystem;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class Player_HealState : PlayerState
{
    private CinemachineImpulseSource impulseSource;
    private float shakeTimer;
    private float shakeCooldown = 0.2f;
    private float shakeForce = 0.06f;

    private ParticleSystem psAttract;
    private ParticleSystem psBurst;

  
    public Player_HealState(Player player, StateMachine stateMachine, string stateName) : base(player, stateMachine, stateName)
    {
        impulseSource = player.GetComponent<CinemachineImpulseSource>();
        psAttract = player.psAttract;
        psBurst = player.psBurst;
     
    }
    public override void Enter()
    {
        base.Enter();
        shakeTimer = 0f;
        triggerCalled = false;
        player.justHealed = false;
        Debug.Log("HealState Enter");
        psAttract.Play();

        AudioSource.PlayClipAtPoint(player.healClip, player.transform.position,2.5f);
    }

    public override void Update()
    {
        base.Update();
        //ˆê’Uƒ{ƒ^ƒ“—£‚·‚Æ‰ñ•œ’†’f‚³‚ê‚é
        if (!player.input.Player.Heal.IsPressed())
        {
            stateMachine.ChangeState(player.idleState);
            return;
        }

        shakeTimer -= Time.deltaTime;
        if (shakeTimer <= 0)
        {
            Vector2 randomDir = Random.insideUnitCircle.normalized;
            impulseSource?.GenerateImpulse(new Vector3(randomDir.x, randomDir.y, 0) * shakeForce);
            shakeTimer = shakeCooldown;
        }
        if (triggerCalled)
        {
            psBurst?.Play();
            player.ApplyHeal();
            player.justHealed = true;
            stateMachine.ChangeState(player.idleState);
            return;
        }
        if (player.health.takeDamage)
        {
            stateMachine.ChangeState(player.idleState);
        }

    }
    public override void Exit()
    {
        Debug.Log("Exit called, stopping psAttract");
        psAttract?.Stop();
        base.Exit();
    }
}
