using System.Collections;
using UnityEngine;

public class Entity_VFX : MonoBehaviour
{
    protected SpriteRenderer sr;
    private Entity entity;
    [Header("On Taking DamageVFX")]
    [SerializeField] private Material onDamageMaterial;
    [SerializeField] private float onDamageVfxDuration = .3f;
    private Material originMaterial;

    private Coroutine onDamageVfxCoroutine;
    [Header("On Doing DamageVFX")]
    [SerializeField] private GameObject hitVfx;
    [SerializeField] private GameObject critHitVfx;

    [Header("Element Colors")]
    [SerializeField] private Color hitVfxColor = Color.white;
    [SerializeField] private Color chillVfx = Color.cyan;
    [SerializeField] private Color burnVfx = Color.red;

    private Color originalHitVfxColor;
    private Coroutine statusVfxCo;
    private void Awake()
    {
        sr = GetComponentInChildren<SpriteRenderer>();
        originMaterial = sr.material;
        entity = GetComponent<Entity>();

        originalHitVfxColor = hitVfxColor;
    }
    public void PlayOnStatusVfx(float duration, ElementType element)
    {
        if (statusVfxCo != null)
            StopCoroutine(statusVfxCo);

        if (element == ElementType.Ice)
            statusVfxCo = StartCoroutine(PlayStatusVfxCo(duration, chillVfx));

        if (element == ElementType.Fire)
            statusVfxCo = StartCoroutine(PlayStatusVfxCo(duration, burnVfx));
    }

    private IEnumerator PlayStatusVfxCo(float duration, Color effectColor)
    {
        float tickInterval = .25f;
        float timePassed = 0;

        Color lightColor = effectColor * 1.2f;
        Color darkColor = effectColor * .8f;

        bool toggle = false;

        while (timePassed < duration)
        {
            sr.color = toggle ? lightColor : darkColor;
            toggle = !toggle;

            yield return new WaitForSeconds(tickInterval);
            timePassed += tickInterval;
        }

        sr.color = originalHitVfxColor;
    }
    public void CreateOnHitVFX(Transform target, bool isCrit,ElementType element)
    {
        GameObject hitPrefab = isCrit ? critHitVfx : hitVfx;
        GameObject vfx = Instantiate(hitPrefab, target.position, Quaternion.identity);
        if(isCrit==false)
        vfx.GetComponentInChildren<SpriteRenderer>().color = GetElementColor(element);

        if (entity.facedir == -1 && isCrit)
            vfx.transform.Rotate(0, 180, 0);
    }
    public void PlayOnDamageVfx()
    {
        if (onDamageVfxCoroutine != null)
            StopCoroutine(onDamageVfxCoroutine);
        onDamageVfxCoroutine = StartCoroutine(OnDamageVfxCo());
    }
    private IEnumerator OnDamageVfxCo()
    {
        sr.material = onDamageMaterial;
        yield return new WaitForSeconds(onDamageVfxDuration);
        sr.material = originMaterial;
    }
    public Color GetElementColor(ElementType element)
    {
        switch (element)
        {
            case ElementType.Ice:
                return chillVfx;

            case ElementType.Fire:
                return burnVfx;

            default:
                return Color.white;
        }
    }
}
