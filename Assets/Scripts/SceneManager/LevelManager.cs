using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance;

    public GameObject transitionsContainer;
    private SceneTransition[] transitions;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        transitions = transitionsContainer.GetComponentsInChildren<SceneTransition>();
    }

    public void LoadScene(string sceneName, string transitionName)
    {
        StartCoroutine(LoadSceneAsync(sceneName, transitionName));
    }

    private IEnumerator LoadSceneAsync(string sceneName, string transitionName)
    {
        SceneTransition transition = transitions.First(t => t.name == transitionName);

        yield return transition.AnimateTransitionIn();
        yield return new WaitForSeconds(0.5f);

        //scene変わってもコインとhealPosionのデータが保存される
        if (Inventory_Player.Instance != null)
        {
            PlayerPrefs.SetInt("SavedGold", Inventory_Player.Instance.gold);

            int healAmount = Inventory_Player.Instance.itemList.FindAll(item => item.itemData.itemName== "Heal").Count;
            PlayerPrefs.SetInt("SavedPotion", healAmount);
       
        }

        AsyncOperation scene = SceneManager.LoadSceneAsync(sceneName);

        scene.allowSceneActivation = false;

        while (scene.progress < 0.9f)
        {
            yield return null;
        }

        scene.allowSceneActivation = true;


        yield return transition.AnimateTransitionOut();
    }
}