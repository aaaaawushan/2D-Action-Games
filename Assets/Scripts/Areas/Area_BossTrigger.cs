using System.Collections;
using Unity.Cinemachine;
using UnityEngine;

public class Area_BossTrigger : MonoBehaviour
{
    private Enemy_Shadow enemy_Shadow;
    private Area_ShowWords showWords;
    [SerializeField] private CinemachineImpulseSource impulseSource;
    [SerializeField] private CanvasGroup bossHealthBarGroup;

    [Header("Look Boss Info")]
    [SerializeField] private CinemachineCamera bossCamera; // Boss専用Cinema Carmera
    [SerializeField] private float cameraHoldTime = 1.5f;

    [Header("BGM")]
    [SerializeField] private AudioSource bossAudioSource;
    [SerializeField] private AudioClip bossClip;
    [SerializeField] private AudioSource bgmSource;
    [SerializeField] private AudioClip bossBGM;

    private bool triggered = false;
    private void Start()
    {
        showWords = GetComponent<Area_ShowWords>();
        enemy_Shadow = FindAnyObjectByType<Enemy_Shadow>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (triggered) return;
        if (collision.CompareTag("Player"))
        {
            triggered = true;
            StartCoroutine(BossIntro(collision.GetComponent<Player>()));
        }
    }
    private IEnumerator BossIntro(Player player)
    {
        player.FreezePlayer(true);
        //カメラがボスの方に近づいく
        bossCamera.Priority = 20;

        impulseSource.GenerateImpulse();
        bossAudioSource.PlayOneShot(bossClip);

        StartCoroutine(showWords.FadeRoutine());
        StartCoroutine(FadeInHealthBar());
        yield return new WaitForSeconds(cameraHoldTime);
        //カメラがプレイヤーの方に戻る
        bossCamera.Priority = 0;

        yield return new WaitForSeconds(0.5f);
        player.FreezePlayer(false);
        enemy_Shadow.StartBattle();
        bgmSource.clip = bossBGM;
        bgmSource.Play();
        GetComponent<Collider2D>().enabled = false;
    }
    private IEnumerator FadeInHealthBar()
    {
        float fadeTime = 1f;
        for (float t = 0; t < fadeTime; t += Time.deltaTime)
        {
            bossHealthBarGroup.alpha = t / fadeTime;
            yield return null;
        }
        bossHealthBarGroup.alpha = 1;
       
    }
}
