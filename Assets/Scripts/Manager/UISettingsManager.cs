using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;

public class UISettingsManager : MonoBehaviour
{
    [Header("main menu")]
    [SerializeField] private GameObject mainMenuPanel;
    [SerializeField] private GameObject settingsButton;
    [Header("settingsPanel")]
    [SerializeField] private GameObject settingsPanel;
    [SerializeField] private GameObject firstInSettings;
    [Header("volumePanel")]
    [SerializeField] private GameObject volumeImage;
    [SerializeField] private GameObject firstInVolume;
    [Header("instructionPanel")]
    [SerializeField] private GameObject instructionImage;
    [SerializeField] private GameObject firstInInstruction;
    [Header("start storyPanel")]
    [SerializeField] private GameObject storyPanel;
    private void Update()
    {
        if (EventSystem.current.currentSelectedGameObject == null)
            EventSystem.current.SetSelectedGameObject(settingsButton);
    }
    public void StartGame()
    {
        //  LevelManager.Instance.LoadScene("Part1", "CrossFade");
        mainMenuPanel.SetActive(false);
        if (storyPanel != null)
        {
            storyPanel.SetActive(true);
            storyPanel.GetComponent<OpeningSequence>().StartOpening();
        }
    }

    public void End()
    {
        Application.Quit();
    }

    public void OpenSettings()
    {
        mainMenuPanel.SetActive(false);
        settingsPanel.SetActive(true);
        StartCoroutine(Focus(firstInSettings));
    }

    public void CloseSettings()
    {
        settingsPanel.SetActive(false);
        mainMenuPanel.SetActive(true);
        EventSystem.current.SetSelectedGameObject(settingsButton);
    }

    public void OpenVolume()
    {
        settingsPanel.SetActive(false);
        volumeImage.SetActive(true);
        StartCoroutine(Focus(firstInVolume));
    }

    public void CloseVolume()
    {
        volumeImage.SetActive(false);
        settingsPanel.SetActive(true);
        StartCoroutine(Focus(firstInSettings));
    }

    public void OpenInstruction()
    {
        settingsPanel.SetActive(false);
        instructionImage.SetActive(true);
        StartCoroutine(Focus(firstInInstruction));
    }

    public void CloseInstruction()
    {
        instructionImage.SetActive(false);
        settingsPanel.SetActive(true);
        StartCoroutine(Focus(firstInSettings));
    }

    private IEnumerator Focus(GameObject target)
    {
        yield return null;
        EventSystem.current.SetSelectedGameObject(target);
    }
}