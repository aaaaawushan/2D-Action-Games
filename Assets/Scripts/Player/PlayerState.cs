using Unity.VisualScripting;
using UnityEngine;

public abstract class PlayerState : EntityState//状态的类型，父类状态，规定：“所有状态都应该长什么样”
                                               //处理通用逻辑（比如随时可以Dash）
{

    protected Player player;//protected:子状态都能直接访问，但外部脚本不能乱动
    protected PlayerInput input;
    protected Player_SkillManager skillManager;

    public PlayerState(Player player, StateMachine stateMachine, string stateName) : base(stateMachine, stateName)//这里的构造函数会提供每个状态他们相对的信息。
    {
        this.player = player;

        anim = player.GetComponentInChildren<Animator>();
        rb = player.rb;
        input = player.input;
        skillManager = player.skillManager;
        stats = player.stats;
    }
    public override void Update()
    {
        base.Update();
        anim.SetFloat("yVelocity", rb.linearVelocity.y);
        if (input.Player.Dash.WasPressedThisFrame() && CanDash())
        {
            skillManager.dash.SetSkillOnCooldown();
            stateMachine.ChangeState(player.dashState);
        }
    }

    private bool CanDash()
    {
        if (skillManager.dash.CanUseSkill() == false) return false;
        if (player.Iswall) return false;
        if (stateMachine.currentState == player.dashState) return false;
        return true;
    }
}
