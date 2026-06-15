using TMPro;
using UnityEngine;

public class Object_Keeper : Object_NPC, IInteractable
{
    private Animator anim;
    private Inventory_Player inventory;
    private Inventory_Storage storage;
    [SerializeField] private string npcName;

    [Header("Message")]
    [SerializeField] private DialogueLine[] dialogueLines;
    [SerializeField] private DialogueLine[] noGoldDialogue;
    [SerializeField] private DialogueLine[] unlockDialogue;

    private bool hasSpoken = false;
    private bool storageUnlocked = false;

    protected override void Awake()
    {
        base.Awake();
        storage = GetComponent<Inventory_Storage>();
        anim = GetComponentInChildren<Animator>();
    }

    public void Interact()
    {
        if (!hasSpoken)
        {
            ui.dialogueUI.StartDialogue(dialogueLines, () =>
            {
                hasSpoken = true;
            });
            return;
        }

        if (!storageUnlocked)
        {
            if (inventory.CanUseGold(10))
            {
                storageUnlocked = true;
                ui.dialogueUI.StartDialogue(unlockDialogue, () =>
                {
                    ui.storageUI.SetupStorage(inventory, storage);
                    ui.storageUI.gameObject.SetActive(true);
                });
            }
            else
            {
                ui.dialogueUI.StartDialogue(noGoldDialogue);
            }
            return;
        }

        bool isActive = ui.storageUI.gameObject.activeSelf;
        ui.storageUI.gameObject.SetActive(!isActive);

    }


    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        base.OnTriggerEnter2D(collision);
        inventory = player.GetComponent<Inventory_Player>();
        storage.SetInventory(inventory);
    }
}