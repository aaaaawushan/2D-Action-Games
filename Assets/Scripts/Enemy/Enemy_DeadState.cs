using UnityEngine;
using System.Collections;
public class Enemy_DeadState : EnemyState
{
    public Enemy_DeadState(Enemy enemy, StateMachine stateMachine, string stateName) : base(enemy, stateMachine, stateName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        enemy.GetComponent<Enemy_GoldDrop>()?.DropGold();
        foreach (var col in enemy.GetComponentsInChildren<Collider2D>())
        {
            if (col.isTrigger)
            {
                col.enabled = false;
            }
        }
        enemy.StartCoroutine(DelayOneSecond());
        stateMachine.SwitchOffStateMachine();
    }
    IEnumerator DelayOneSecond()
    {
        yield return new WaitForSeconds(1.5f);
        Object.Destroy(enemy.gameObject);
    }
    public override void Exit()
    {
        base.Exit();
        // enemy.gameObject.SetActive(false);
    }
}
