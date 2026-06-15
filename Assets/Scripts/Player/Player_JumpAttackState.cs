using UnityEngine;
using UnityEngine.Windows;

public class Player_JumpAttackState : PlayerState
{
    private bool touchedGround;
    public Player_JumpAttackState(Player player, StateMachine stateMachine, string stateName) : base(player, stateMachine, stateName)
    {
    }
    public override void Enter()
    {
        base.Enter();
        touchedGround = false;
    }
    public override void Update()
    {
        base.Update();
        if (!touchedGround)
        {
            float xInput = player.moveInput.x;
            player.SetVelocity(xInput * player.moveSpeed * player.inAirMoveMultiplier, rb.linearVelocity.y);
        }

        if (player.Isgrounded&&touchedGround==false)
        {
            touchedGround = true;
            anim.SetTrigger("jumpAttackTrigger");
            player.SetVelocity(0, rb.linearVelocity.y);
        }
        
        if (triggerCalled && player.Isgrounded)
        {
            stateMachine.ChangeState(player.idleState);
        }
    
    }
}
