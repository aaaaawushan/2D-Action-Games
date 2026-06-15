using System.Xml;
using UnityEngine;

public class Player_GroundedState : PlayerState
{
    public Player_GroundedState(Player player, StateMachine stateMachine, string stateName) : base(player, stateMachine, stateName)
    {
    }
    public override void Update()
    {
        base.Update();
        if (rb.linearVelocity.y < 0 && player.Isgrounded == false)
        {
            stateMachine.ChangeState(player.fallState);
        }
        //コヨーテタイム内かつジャンプバッファ内ならジャンプ
        if (player.jumpBufferCounter > 0 && player.coyoteTimeCounter > 0)
        {
            player.jumpBufferCounter = 0;
            stateMachine.ChangeState(player.jumpState);
        }
        if (input.Player.Attack.WasPerformedThisFrame())
        {
            stateMachine.ChangeState(player.basicAttackState);
        }

    }


}
