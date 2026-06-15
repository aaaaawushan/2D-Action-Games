using UnityEngine;

public class Player_WallSliedState : PlayerState
{
    public Player_WallSliedState(Player player, StateMachine stateMachine, string stateName) : base(player, stateMachine, stateName)
    {
    }
    public override void Update()
    {
        base.Update();
        HandleWallSlide();
        if (player.Isgrounded)
        {
            stateMachine.ChangeState(player.idleState);
            player.Flip();//可以保证滑墙结束时方向对着墙的反方向
        }

        if (player.Iswall == false)
        {
            stateMachine.ChangeState(player.fallState);
        }
        if (input.Player.Jump.WasPerformedThisFrame())
        {
            stateMachine.ChangeState(player.wallJumpState);
        }
    }

    private void HandleWallSlide()
    {
        if (player.moveInput.y < 0)
        {
            player.SetVelocity(player.moveInput.x, rb.linearVelocity.y);
        }
        else
        {
            player.SetVelocity(player.moveInput.x, rb.linearVelocity.y * player.wallSliderSlowMultiplier);
        }
    }
}
