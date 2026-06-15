using UnityEngine;

public class Object_Gate : Object_NPC, IInteractable
{
    private Inventory_Player inventory;

    [SerializeField] private string sceneName;
    [SerializeField] private string transitionName;

    [Header("Dialogue")]
    [SerializeField] private DialogueLine[] noDiamondDialogue;
    [SerializeField] private DialogueLine[] useDiamondDialogue;
    protected override void Update()
    {
        HandleToolTipFloat();
    }
    public void Interact()
    {
        if (inventory.hasDiamond())
        {
            ui.dialogueUI.StartDialogue(useDiamondDialogue, () =>
                {
                    inventory.usedDiamond();
                    LevelManager.Instance.LoadScene(sceneName, transitionName);
                });
        }
        else
        {
            ui.dialogueUI.StartDialogue(noDiamondDialogue);
        }
    
    }
    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        base.OnTriggerEnter2D(collision);
        inventory = player.GetComponent<Inventory_Player>();
    }
}
