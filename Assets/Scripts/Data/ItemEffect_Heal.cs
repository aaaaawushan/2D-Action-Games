using UnityEngine;

[CreateAssetMenu(menuName = "RPG Setup/Item Data/Item effect/Heal effect", fileName = "Item effect data - heal")]
//プレイヤーのMPゲージを回復
public class ItemEffect_Heal : ItemEffect_DataSO
{
    [SerializeField] private float healPercent = .5f;

    public override void ExecuteEffect()
    {
        Player player = FindFirstObjectByType<Player>();
        float restore = player.maxMana * healPercent;

        player.currentMana = Mathf.Min(player.currentMana + restore, player.maxMana);
        player.health.UpdateManaBar();
    }
}