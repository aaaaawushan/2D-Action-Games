using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class BackToStart : MonoBehaviour
{
    public void GoToStart()
    {
        SceneManager.LoadScene(0);
    }
}
