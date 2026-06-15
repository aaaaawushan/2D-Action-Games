using UnityEngine;

public abstract class EntityState
{
    protected Entity_Stats stats;
    protected Animator anim;
    protected StateMachine stateMachine;
    protected string animBoolName; 
    protected Rigidbody2D rb;
    protected float stateTimer;

    protected bool triggerCalled;


    public EntityState(StateMachine stateMachine, string stateName)
    {
        this.stateMachine = stateMachine;
        this.animBoolName = stateName;
    }
    public virtual void Enter()
    {
        anim.SetBool(animBoolName, true);
        triggerCalled = false;
    }
    public virtual void Update()
    {
        stateTimer -= Time.deltaTime;
    }
    public virtual void Exit()
    {
        anim.SetBool(animBoolName, false);
    }
    public virtual void CallAnimationTrigger()
    {
        triggerCalled = true;
    }
    public void SynAttackSpeed()
    {
        float attackSpeed = stats.offense.atackSpeed.GetValue();
        anim.SetFloat("attackSpeedMultipler", attackSpeed);

    }
  
}
