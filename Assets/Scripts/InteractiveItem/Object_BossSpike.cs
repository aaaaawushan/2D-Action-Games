using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class Object_BossSpike : Object_Spike
{
    [SerializeField] private float moveDistance = 1.3f;
    [SerializeField] private float moveSpeed = 3f;

    private Vector3 hiddenPos;
    private Vector3 shownPos;
    private bool isHiding = false;

    protected override void Start()
    {
        base.Start();
        shownPos = transform.position;
        hiddenPos = transform.position - new Vector3(0, moveDistance, 0);
        transform.position = hiddenPos;
    }

    private void Update()
    {
        if (!isHiding)
        {
            transform.position = Vector3.MoveTowards(transform.position, shownPos, moveSpeed * Time.deltaTime);
        }
        else
        {
            transform.position = Vector3.MoveTowards(transform.position, hiddenPos, moveSpeed * Time.deltaTime);
            if (transform.position == hiddenPos)
                Destroy(gameObject);
        }
    }

    public void SpikeHide()
    {
        isHiding = true;
    }

}
