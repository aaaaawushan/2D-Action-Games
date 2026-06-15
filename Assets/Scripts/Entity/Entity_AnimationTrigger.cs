using Unity.VisualScripting;
using UnityEngine;

public class Entity_AnimationTrigger : MonoBehaviour
{
    private Entity entity;
    private Entity_Combat entityCombat;
    private Player player;
    protected virtual void Awake()
    {
        entity = GetComponentInParent<Entity>();
        entityCombat = GetComponentInParent<Entity_Combat>();
        player = GetComponentInParent<Player>();
    }
    //アニメーションの重要なタイミングを現在のステートに通知する
    public void CurrentStateTrigger()
    {
        entity.CurrentStateAnimationTrigger();
    }
    public void AttackTrigger()
    {
        Debug.Log("AttackTrigger called!");
        entityCombat.PerformAttack();
    }
  
}
