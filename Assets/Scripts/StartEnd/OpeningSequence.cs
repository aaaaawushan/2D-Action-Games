using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class OpeningSequence : MonoBehaviour
{
    [SerializeField] private CanvasGroup blackScreen;
    [Header("text Info")]
    [SerializeField] private TextMeshProUGUI storyText;
    [SerializeField] private float textSpeed;
    private string[] pages = {
        "ここは\nゲームのオープニングテキストのはずですが",
        "何をかくか\nまだかんがえています",
        "一旦\nこのままにします"
    };
    private bool isType;
    private bool isPageComplete;
    private bool isSkip;
    private float inputCooldown = 0f;

    public void StartOpening()
    {
        StartCoroutine(PlayOpening());
    }

    private void Update()
    {
        if (inputCooldown > 0)
        {
            inputCooldown -= Time.deltaTime;
            return;
        }

        bool anyInput = Input.GetKeyDown(KeyCode.Return) ||
                        Input.GetKeyDown(KeyCode.Space) ||
                        Input.GetKeyDown(KeyCode.JoystickButton1) ||
                        Input.GetMouseButtonDown(0);

        if (anyInput)
        {
            if (isType) isSkip = true;
            else if (isPageComplete)
            {
                isPageComplete = false;
                inputCooldown = 0.3f;
            }
        }
    }

    private IEnumerator PlayOpening()
    {
        blackScreen.alpha = 0f;
        yield return FadeCanvasGroup(blackScreen, 0f, 1f, 1f);
        foreach (var page in pages)
        {
            storyText.text = "";
            isSkip = false;
            isPageComplete = false;
            inputCooldown = 0.5f;
            yield return StartCoroutine(TypeText(page));
            isPageComplete = true;
            yield return new WaitUntil(() => !isPageComplete);

            yield return FadeCanvasGroup(storyText.GetComponent<CanvasGroup>(), 1f, 0f, 0.5f);
            storyText.text = "";
            yield return FadeCanvasGroup(storyText.GetComponent<CanvasGroup>(), 0f, 1f, 0.3f);
        }
        LevelManager.Instance.LoadScene("Part1", "CrossFade");
    }

    private IEnumerator TypeText(string text)
    {
        isType = true;
        foreach (var t in text)
        {
            if (isSkip)
            {
                storyText.text = text;
                break;
            }
            storyText.text += t;
            yield return new WaitForSeconds(textSpeed);
        }
        isType = false;
    }

    private IEnumerator FadeCanvasGroup(CanvasGroup cg, float from, float to, float duration)
    {
        float t = 0;
        while (t < duration)
        {
            t += Time.deltaTime;
            cg.alpha = Mathf.Lerp(from, to, t / duration);
            yield return null;
        }
        cg.alpha = to;
    }
}