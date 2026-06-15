using System.Collections;
using DG.Tweening;
using UnityEngine;

public class DeathZone : MonoBehaviour
{
    [SerializeField] private CrossFade crossFade;
    private Player player;
    private bool isRespawning = false;

    private void Start()
    {
        player = FindAnyObjectByType<Player>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (player.health.IsDead) return;
        if (collision.GetComponent<Player>() != null && !isRespawning)
        {
            StartCoroutine(RespawnRoutine());
        }
    }

    private IEnumerator RespawnRoutine()
    {
        isRespawning = true;
        player.FreezePlayer(true);

        crossFade.crossFade.DOFade(1f, 0.3f);
        yield return new WaitForSeconds(0.3f);

        player.TeleportPlayer(CheckpointManager.Instance.lastCheckpoint);
        player.rb.linearVelocity = Vector2.zero;
        player.rb.gravityScale = 0;
        crossFade.crossFade.DOFade(0f, 0.3f);
        yield return new WaitForSeconds(0.3f);

        player.FreezePlayer(false);
        isRespawning = false;
    }
}