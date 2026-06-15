using UnityEngine;

public class Object_Merchant : Object_NPC, IInteractable
{
    private Animator anim;
    private Inventory_Player playerInventory;
    private Inventory_Merchant merchant;

    [SerializeField] private DialogueLine[] dialogueLines;
    protected override void Awake()
    {
        base.Awake();
        merchant = GetComponent<Inventory_Merchant>();
        anim = GetComponentInChildren<Animator>();
    }

    public void Interact()
    {
        bool isActive = ui.merchantUI.gameObject.activeSelf;

        if (isActive)
        {
            ui.merchantUI.gameObject.SetActive(false);
            return;
        }

        ui.dialogueUI.StartDialogue(dialogueLines, () =>
        {
            ui.merchantUI.SetupShop(playerInventory, merchant);
            ui.merchantUI.gameObject.SetActive(true);
        });
    }

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        base.OnTriggerEnter2D(collision);
        playerInventory = player.GetComponent<Inventory_Player>();
        merchant.SetInventory(playerInventory);
    }
}