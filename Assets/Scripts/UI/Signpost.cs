using UnityEngine;

public class Signpost : MonoBehaviour
{
    [SerializeField] private GameObject dialogueUI;

    void Start()
    {
        if (dialogueUI != null)
        {
            dialogueUI.SetActive(false);
        }

    }

    // Update is called once per frame
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            ShowDialogue();
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            ExitDialogue();
        }
    }

    private void ShowDialogue()
    {
        if (dialogueUI != null)
        {
            dialogueUI.SetActive(true);
        }
    }
    private void ExitDialogue()
    {
        if (dialogueUI != null)
        {
            dialogueUI.SetActive(false);
        }
    }
}
