using UnityEngine;

public class Player_DashState : PlayerState
{
    private float originalGravityScale;
    private int dashDir;
    public Player_DashState(Player player, StateMachine stateMachine, string stateName) : base(player, stateMachine, stateName)
    {
    }
    public override void Enter()
    {
        Debug.Log($"DashState Enter - moveInput: {player.moveInput}");
        base.Enter();

        skillManager.dash.OnStartEffect();
        player.vfx.DoImageEchoEffect(player.dashDuration);

        if (Mathf.Abs(player.moveInput.x) > 0.125f)
        {
            dashDir = player.moveInput.x > 0 ? 1 : -1;
        }
        else
        {
            dashDir = player.facedir;
        }
        stateTimer = player.dashDuration;
        originalGravityScale = rb.gravityScale;
        rb.gravityScale = 0;
    }
    public override void Update()
    {
        base.Update();
        CancelDashIFNeeded();
        player.SetVelocity(player.dashSpeed * dashDir, 0);
        if (stateTimer < 0)
        {if (player.Isgrounded)
                stateMachine.ChangeState(player.idleState);
            else
                stateMachine.ChangeState(player.fallState);
        }
    }
    public override void Exit()
    {
        base.Exit();

        skillManager.dash.OnEndEffect();

        player.SetVelocity(0, 0);
        rb.gravityScale = originalGravityScale;
    }
    private void CancelDashIFNeeded()
    {
        if (player.Iswall)
        {
            if (player.Isgrounded) stateMachine.ChangeState(player.idleState);
            else stateMachine.ChangeState(player.wallSliedState);
        }
    }
}
