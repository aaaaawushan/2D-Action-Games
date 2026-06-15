using UnityEngine;

public class Enemy_ShadowDeadState : Enemy_ShadowState
{
    public Enemy_ShadowDeadState(Enemy_Shadow shadowEnemy, StateMachine stateMachine, string stateName) : base(shadowEnemy, stateMachine, stateName)
    {
    }
    public override void Enter()
    {
        base.Enter();
        enemy.SetVelocity(0, 0);
        shadowEnemy.GetComponent<Collider2D>().enabled = false;
        //shadowEnemy.GetComponent<Enemy_GoldDrop>()?.DropGold();
    }

    public override void Update()
    {
        base.Update();
      
    }
    public override void CallAnimationTrigger()
    {
        LevelManager.Instance.LoadScene("End", "CrossFade");
        Object.Destroy(shadowEnemy.gameObject);
    }
}
