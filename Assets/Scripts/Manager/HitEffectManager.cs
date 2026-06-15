using UnityEngine;
using System.Collections;
using Unity.Cinemachine;

public class HitEffectManager : MonoBehaviour
{
    [SerializeField] private float hitlagTime = 0.06f;
    private bool isPaused;

    [Header("Camera Shake")]
    [SerializeField] private CinemachineImpulseSource impulseSource;
    [SerializeField] private float shakeForce = 0.03f;


    public static HitEffectManager instance;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
            Destroy(gameObject);
    }
    public void CameraShake()
    {
        impulseSource.GenerateImpulse(shakeForce);
    }
    public void TriggerHitlag()
    {
        Debug.Log("TriggerHitLag");
        if (!isPaused)
            StartCoroutine(HitlagCoroutine());
    }

    IEnumerator HitlagCoroutine()
    {
        isPaused = true;
        Time.timeScale = 0f;
        Debug.Log("timeScale set to 0");
        yield return new WaitForSecondsRealtime(hitlagTime);
        Debug.Log("timeScale restored to 1");
        Time.timeScale = 1f;
        isPaused = false;
    }
}
