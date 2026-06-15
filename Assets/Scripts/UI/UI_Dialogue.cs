using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class UI_Dialogue : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI npcNameText;
    [SerializeField] private TextMeshProUGUI dialogueText;
    [SerializeField] private TextMeshProUGUI promptText;

    private DialogueLine[] lines;
    private int currentLine = 0;
    private System.Action onDialogueEnd;


    private void Update()
    {
        if (!gameObject.activeSelf) return;
        if (Input.GetKeyDown(KeyCode.E) || Gamepad.current.buttonEast.wasPressedThisFrame)
            NextLine();
    }

    public void StartDialogue(DialogueLine[] dialogueLines, System.Action onEnd = null)
    {
        lines = dialogueLines;
        currentLine = 0;
        onDialogueEnd = onEnd;
        gameObject.SetActive(true);
        ShowLine();
    }
  
    public void NextLine()
    {
        currentLine++;
        if (currentLine >= lines.Length)
        {
            EndDialogue();
            return;
        }
        ShowLine();
    }
    private void ShowLine()
    {
        npcNameText.text = lines[currentLine].speakerName;
        dialogueText.text = lines[currentLine].text;
    }
    private void EndDialogue()
    {
        gameObject.SetActive(false);
        onDialogueEnd?.Invoke();
    }
}