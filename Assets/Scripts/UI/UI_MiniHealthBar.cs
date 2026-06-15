using UnityEngine;

public class UI_MiniHealthBar : MonoBehaviour
{
    [SerializeField] private Entity entity;
    private void OnEnable()
    {
       // entity.OnFlipped += HandleFlip;
    }
    private void OnDisable()
    {
      //  entity.OnFlipped -= HandleFlip;
    }
    // Update is called once per frame
    void HandleFlip()
    {
        transform.rotation = Quaternion.identity;
    }
}
