using UnityEngine;

public class Enemy_BattleState : EnemyState
{
    private Transform player;
    private float lastTimeWasInBattle;
    
    public Enemy_BattleState(Enemy enemy, StateMachine stateMachine, string stateName) : base(enemy, stateMachine, stateName)
    {
    }
    public override void Enter()
    {
        base.Enter();
        Debug.Log("its battle state now");

        if (enemy.player != null)
        {
            player = enemy.player; 
        }
        else
        {
            var hit = enemy.PlayerDetection();
            if (hit.collider != null)
                player = hit.transform; 
        }
        if (ShouldRetreat())
        {
            rb.linearVelocity = new Vector2(enemy.retreatVelocity.x * -DirectionToPlayer(), enemy.retreatVelocity.y);
            enemy.HandleFlip(DirectionToPlayer());
        }
    }
    public override void Update()
    {
        base.Update();

        if (enemy.PlayerDetection()) lastTimeWasInBattle = Time.time;

        if (BattleTimeOver())
        {
            stateMachine.ChangeState(enemy.idleState);
        }
        if (WithinAttackRange())
        {
            stateMachine.ChangeState(enemy.attackState);
        }
        else
            enemy.SetVelocity(enemy.battleMoveSpeed * DirectionToPlayer(), rb.linearVelocity.y);
        if (enemy.Isgrounded == false || enemy.Iswall)
        {
            stateMachine.ChangeState(enemy.idleState);
        }
    }
    private float DistanceToPlayer()
    {
        if (player == null) return float.MaxValue;
        return Mathf.Abs(player.position.x - enemy.transform.position.x);
    }
    private bool BattleTimeOver() => lastTimeWasInBattle + enemy.battleTimerDuration < Time.time;
    private bool WithinAttackRange()
    {
        return DistanceToPlayer() < enemy.attackDistance;
    }
    private bool ShouldRetreat() => DistanceToPlayer() < enemy.minRereatDistance;
   private int DirectionToPlayer()
    {
        if (player == null) return 0;
        return player.position.x > enemy.transform.position.x ? 1 : -1;
    }
    public override void Exit()
    {
        base.Exit();
       
    }


}

