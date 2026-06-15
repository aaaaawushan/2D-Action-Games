using UnityEngine;

public class Enemy_FlyDeadState : Enemy_FlyingState
{
    private bool hasLanded;
    private float destroyDelay = 0.6f; 

    public Enemy_FlyDeadState(Enemy_Flying enemy, StateMachine stateMachine, string stateName)
        : base(enemy, stateMachine, stateName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        flyingEnemy.StopAllCoroutines();
        flyingEnemy.GetComponent<Enemy_GoldDrop>()?.DropGold();
        foreach (var col in flyingEnemy.GetComponents<Collider2D>())
        {
           col.enabled = false;
        }
        rb.gravityScale = 4f;
        hasLanded = false;
    }

    public override void Update()
    {
        base.Update();
        if (!hasLanded && flyingEnemy.Isgrounded)
        {
            hasLanded = true;
            rb.linearVelocity = Vector2.zero;
            rb.gravityScale = 0f; 
            flyingEnemy.StartCoroutine(DestroyAfterDelay());
        }
    }

    public override void CallAnimationTrigger()
    {
        stateMachine.SwitchOffStateMachine();
        flyingEnemy.StartCoroutine(DestroyAfterDelay());
    }

    private System.Collections.IEnumerator DestroyAfterDelay()
    {
        yield return new WaitForSeconds(destroyDelay);
        Object.Destroy(flyingEnemy.gameObject);
    }
}