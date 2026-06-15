using UnityEngine;

public class Player_WallJumpState : PlayerState
{
    public Player_WallJumpState(Player player, StateMachine stateMachine, string stateName) : base(player, stateMachine, stateName)
    {
    }
    public override void Enter()
    {
        base.Enter();
        stateTimer = 0.1f; 
        player.SetVelocity(player.wallJumpForce.x * -player.facedir, player.wallJumpForce.y);
    }

    public override void Update()
    {
        base.Update();
        if (rb.linearVelocity.y < 0)
        {
            stateMachine.ChangeState(player.fallState);
        }
        if (player.Iswall && stateTimer < 0) 
        {
            stateMachine.ChangeState(player.wallSliedState);
        }
    }

}
