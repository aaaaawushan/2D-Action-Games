using Unity.Cinemachine;
using UnityEngine;

public class CameraLookDown : MonoBehaviour
{
    [SerializeField] private float lookDownOffset = 4f;
    [SerializeField] private float lookDownSpeed = 6f;
    [SerializeField] private float returnSpeed = 3f;

    [SerializeField] private CinemachineCamera vcam;
    private Player player;

    private CinemachinePositionComposer composer;
    private Vector3 originalOffset;

    private void Start()
    {
        vcam = FindAnyObjectByType<CinemachineCamera>();
        if (vcam == null) return;

        composer = vcam.GetComponent<CinemachinePositionComposer>();
        player = GetComponent<Player>();
        originalOffset = composer.TargetOffset;
    }

    private void Update()
    {
        if (composer == null) return;
        bool shouldLookDown = (Input.GetKey(KeyCode.S) || player.moveInput.y < -0.5f) && !player.IsWallSliding();

        Vector3 targetOffset = shouldLookDown
            ? new Vector3(0, -lookDownOffset, 0)
            : originalOffset;

        composer.TargetOffset = Vector3.Lerp(composer.TargetOffset, targetOffset,
      (shouldLookDown ? lookDownSpeed : returnSpeed) * Time.deltaTime);

    }
}