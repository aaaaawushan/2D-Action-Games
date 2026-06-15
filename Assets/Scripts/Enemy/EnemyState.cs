using UnityEngine;

public class EnemyState : EntityState
{
    protected Enemy enemy;
    public EnemyState(Enemy enemy,StateMachine stateMachine, string stateName) : base(stateMachine, stateName)
    {
        this.enemy = enemy;
        anim = enemy.anim;
        rb = enemy.rb;
        stats = enemy.stats;
    }
    public override void Update()
    {
        base.Update();
       
        anim.SetFloat("moveAnimSpeedMultiplier", enemy.moveAnimSpeedMultiplier);
        anim.SetFloat("xVelocity", rb.linearVelocity.x);
    }
}
