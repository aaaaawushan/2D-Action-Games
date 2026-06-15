using UnityEngine;

public class StateMachine//单纯提供了切换状态的方式,因为Entity没有 MonoBehaviour，无法自己切换方法，所以新建这个脚本去运行Entity的方法们。
{
    public EntityState currentState { get; private set; }
    public bool canChangeState;
    public void Initialize(EntityState startState)
    {
        canChangeState = true;
        currentState = startState;
        currentState.Enter();
    }
    public void ChangeState(EntityState newState)
    {if (canChangeState == false) return;
        currentState.Exit();
        currentState = newState;
        currentState.Enter();
    }
    public void UpdateActiveState()
    {
        currentState.Update();
    }
    public void SwitchOffStateMachine() => canChangeState = false;
}
