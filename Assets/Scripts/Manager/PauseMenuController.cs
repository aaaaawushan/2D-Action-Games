using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PauseMenuController : MonoBehaviour
{
    [SerializeField] private GameObject settingsPanel;
    [SerializeField] private GameObject setting;
    [SerializeField] private Image InstructionImage;
    private bool isSettingsOpen = false;
    [SerializeField] private string startScene;


    [Header("first Button")]
    [SerializeField] private Button firstSelectedButton;
    [SerializeField] private Button instructionBackButton;


    private void Start()
    {
        settingsPanel.SetActive(false);
        InstructionImage.gameObject.SetActive(false);
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.JoystickButton7))
        {
            ToggleSettings();
        }
    }

    public void ToggleSettings()
    {
        isSettingsOpen = !isSettingsOpen;
        settingsPanel.SetActive(isSettingsOpen);

        Time.timeScale = isSettingsOpen ? 0 : 1;

        if (isSettingsOpen && firstSelectedButton != null)
        {
            EventSystem.current.SetSelectedGameObject(firstSelectedButton.gameObject);
        }
    }

    public void Controls()
    {
        setting.SetActive(false);
        InstructionImage.gameObject.SetActive(true);
        if (instructionBackButton != null)
        {
            EventSystem.current.SetSelectedGameObject(instructionBackButton.gameObject);
        }
    }

    public void ReturnToMainMenu()
    {
        Time.timeScale = 1;
        UnityEngine.SceneManagement.SceneManager.LoadScene(startScene);
    }
    public void CloseSettings()
    {
        isSettingsOpen = false;
        settingsPanel.SetActive(false);
        Time.timeScale = 1;
    }
    public void CloseInstruction()
    {
        setting.SetActive(true);
        InstructionImage.gameObject.SetActive(false);
        if (firstSelectedButton != null)
        {
            EventSystem.current.SetSelectedGameObject(firstSelectedButton.gameObject);
        }
    }

}