using UnityEngine;
using UnityEngine.SceneManagement;

public class Retry : MonoBehaviour
{
    [SerializeField] private string sceneName;
    private bool canRetry = false;

    private void OnEnable()
    {
        canRetry = false;
        Invoke(nameof(AllowRetry), 0.2f);
    }

    private void AllowRetry() => canRetry = true;

    public void RetryToMain()
    {
        SceneManager.LoadScene(sceneName);
    }

    void Update()
    {
        if (!canRetry) return;
        if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.JoystickButton1))
        {
            RetryToMain();
        }
    }
}