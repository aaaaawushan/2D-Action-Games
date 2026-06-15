using UnityEngine;

public class Player_Fallstate : Player_AiredState
{
    public Player_Fallstate(Player player, StateMachine stateMachine, string stateName) : base(player, stateMachine, stateName)
    {
    }
    public override void Update()
    {
        base.Update();

        // コヨーテタイム内ならジャンプ可能
        if (player.jumpBufferCounter > 0 && player.coyoteTimeCounter > 0)
        {
            player.jumpBufferCounter = 0;
            stateMachine.ChangeState(player.jumpState);
        }

        if (player.Isgrounded)
        {
            stateMachine.ChangeState(player.idleState);
        }
        if (player.Iswall)
        {
            stateMachine.ChangeState(player.wallSliedState);
        }
    }
    
}
