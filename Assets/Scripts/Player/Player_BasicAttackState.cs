using UnityEngine;

public class Player_BasicAttackState : PlayerState
{
    private static int FirstComboIndex = 1;
    private float attackTimer;
    private int comboIndex = 1;
    private int comboLimit = 3;
    private int attackDir;
    private float lastTimeAttacked;
    private bool comboAttackQueued;
    public Player_BasicAttackState(Player player, StateMachine stateMachine, string stateName) : base(player, stateMachine, stateName)
    {
        if (comboLimit != player.attackVelocity.Length)
        {
            comboLimit = player.attackVelocity.Length;
        }
    }
    public override void Enter()
    {
        base.Enter();
        player.SetVelocity(0, rb.linearVelocity.y);
        comboAttackQueued = false;
        ResetComboIndexIfNeed();

        if (player.moveInput.x != 0)
        {
            attackDir = ((int)player.moveInput.x);
        }
        else { attackDir = player.facedir; }

          anim.SetInteger("basicAttackIndex", comboIndex);

        ApplyAttackVelocity();
        SynAttackSpeed();
    }

    public override void Update()
    {
        base.Update();
        HandleAttackVelocity();
        if (input.Player.Attack.WasPressedThisFrame())
        {
            QueueNextAttack();
        }
       

        if (triggerCalled)
        {
            HandleStateExit();

        }
    }

    private void HandleStateExit()
    {
        if (comboAttackQueued)
        {
            anim.SetBool(animBoolName, false);
            player.EnterAttackStateWithDelay();
        }
        else
        {
            stateMachine.ChangeState(player.idleState);
        }
    }

    public void QueueNextAttack()
    {
        if (comboIndex < comboLimit)
        {
            comboAttackQueued = true;
        }
    }
    public override void Exit()
    {
        base.Exit();
        comboIndex++;
        lastTimeAttacked = Time.time;
    }
    public void HandleAttackVelocity()
    {
        attackTimer -= Time.deltaTime;


        if (attackTimer < 0)
            player.SetVelocity(0, rb.linearVelocity.y);
    }

    private void ApplyAttackVelocity()
    {
        attackTimer = player.attackDuration;
        Vector2 attackVelocity = player.attackVelocity[comboIndex - 1];
        player.SetVelocity(attackVelocity.x * player.facedir, attackVelocity.y);
    }
    private void ResetComboIndexIfNeed()
    {
        if (comboIndex > comboLimit)
        {
            comboIndex = FirstComboIndex;
        }
        if (Time.time > lastTimeAttacked + player.comboResetTime)
        {
            comboIndex = FirstComboIndex;
        }

    }

}
