using UnityEngine;

public class Player_MoveState : Player_GroundedState
{
    public Player_MoveState(Player player, StateMachine stateMachine, string stateName) : base(player, stateMachine, stateName)
    {
    }
    public override void Update()
    {
        base.Update();
        if (Mathf.Abs(player.moveInput.x) < 0.2f)
        {
            stateMachine.ChangeState(player.idleState);
        }
        player.SetVelocity(player.moveInput.x * player.moveSpeed, rb.linearVelocity.y);
    }
}
