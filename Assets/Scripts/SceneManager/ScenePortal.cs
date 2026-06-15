using UnityEngine;

public class ScenePortal : MonoBehaviour
{
    [SerializeField] private string sceneName;
    [SerializeField] private string transitionName;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            LevelManager.Instance.LoadScene(sceneName, transitionName);
        }
    }
}