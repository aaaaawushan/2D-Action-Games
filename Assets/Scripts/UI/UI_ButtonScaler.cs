using UnityEngine;
using UnityEngine.EventSystems;

public class UIButtonScaler : MonoBehaviour,
    IPointerEnterHandler,
    IPointerExitHandler,
    ISelectHandler,    // キーボード選択時
    IDeselectHandler   // キーボード移動時
{
    private Vector3 originScale;
    [SerializeField] private Vector3 targetScale = new Vector3(1.1f, 1.1f, 1f);
    [SerializeField] private float lerpSpeed = 10f;
    private bool isHovered = false;

    [Header("Music Clip")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip hoverClip;

    private float lastPlayTime = -999f;
    [SerializeField] private float playCooldown = 0.5f;

    void Start()
    {
        originScale = transform.localScale;
    }

    void Update()
    {
        Vector3 target = isHovered ? targetScale : originScale;
        if (Vector3.Distance(transform.localScale, target) > 0.01f)
        {
            transform.localScale = Vector3.Lerp(transform.localScale, target, lerpSpeed * Time.unscaledDeltaTime);
        }
    }

    // ホバー開始・キーボード選択
    private void OnActivate()
    {
        isHovered = true;
        if (audioSource != null && hoverClip != null && Time.unscaledTime - lastPlayTime > playCooldown)
        {
            audioSource.PlayOneShot(hoverClip);
            lastPlayTime = Time.unscaledTime;
        }
    }

    // ホバー終了・キーボード移動
    private void OnDeactivate()
    {
        isHovered = false;
    }

    public void OnPointerEnter(PointerEventData eventData) => OnActivate();
    public void OnPointerExit(PointerEventData eventData) => OnDeactivate();
    public void OnSelect(BaseEventData eventData) => OnActivate();
    public void OnDeselect(BaseEventData eventData) => OnDeactivate();
}