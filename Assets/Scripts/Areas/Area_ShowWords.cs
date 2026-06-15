using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

//Boss名前の表示
public class Area_ShowWords : MonoBehaviour
{
    [SerializeField] private TextMeshPro wordsToShow;
    [SerializeField] private float fadeInTime=0.8f;
    [SerializeField] private float fadeOutTime = 0.8f;
    [SerializeField] private float stayTime=1f;
    [SerializeField] private bool triggerByPlayer = true; // ボス戦の時no check

    private Coroutine currentRoutine;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!triggerByPlayer) return;
        if (collision.CompareTag("Player"))
        {
            if (currentRoutine != null) StopCoroutine(currentRoutine);
            currentRoutine = StartCoroutine(FadeRoutine());
        }
    }


    public IEnumerator FadeRoutine()
    {
        Color c = wordsToShow.color;
        for (float t = 0; t < fadeInTime; t += Time.deltaTime)
        {
            c.a = t / fadeInTime;
            wordsToShow.color = c;
            yield return null;
        }
        c.a = 1;
        wordsToShow.color = c;
        yield return new WaitForSeconds(stayTime);

        for (float t = 0; t < fadeOutTime; t += Time.deltaTime)
        {
            c.a = 1f - t / fadeOutTime;
            wordsToShow.color = c;
            yield return null;
        }
        c.a = 0f;
        wordsToShow.color = c;

    }
}
