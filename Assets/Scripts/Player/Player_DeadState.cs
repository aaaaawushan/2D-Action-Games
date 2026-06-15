using UnityEngine;

public class Player_DeadState : PlayerState
{

    public Player_DeadState(Player player, StateMachine stateMachine, string stateName) : base(player, stateMachine, stateName)
    {

    }
    public override void Enter()
    {
        base.Enter();
        player.rb.linearVelocityX = 0;
        player.rb.gravityScale = 2;
        input.Disable();
        Inventory_Player inventory = player.GetComponent<Inventory_Player>();

        int goldToLose = Mathf.RoundToInt(inventory.GetGold() * Random.Range(0.3f, 0.6f));
        inventory.RemoveGold(goldToLose);

        PlayerPrefs.SetInt("SavedGold", inventory.GetGold());
        PlayerPrefs.Save();

        inventory.ClearInventory();
    }
    public override void Update()
    {
        base.Update();

        if (player.Isgrounded)
        {
            rb.simulated = false;
            player.retryButton.gameObject.SetActive(true);
        }
    }
}

