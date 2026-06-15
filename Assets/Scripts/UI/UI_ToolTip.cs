using UnityEngine;

public class UI_ToolTip : MonoBehaviour
{
    private RectTransform rect;
    [SerializeField] private Vector2 offset = new Vector2(300, 20);

    protected virtual void Awake()
    {
        rect = GetComponent<RectTransform>();
        ShowToolTip(false, rect);
    }
    public virtual void ShowToolTip(bool show, RectTransform targetRect)
    {
        if (show == false)
        {
            rect.position = new Vector2(9999, 9999);
        }
        else
            UpdatePosition(targetRect);

    }
    private void UpdatePosition(RectTransform targetRect)
    {
        float screenCenterX = Screen.width / 2;
        float screenTop = Screen.height;
        float screenBottom = 0;
        Vector2 targetPosition = targetRect.position;
        targetPosition.x = targetPosition.x > screenCenterX ? targetPosition.x - offset.x : targetPosition.x + offset.x;

        float veritcalHalf = rect.sizeDelta.y / 2f;//¼ÆËãÍ¼Æ¬Ò»°EÄ¸ß¶È
        float topY = veritcalHalf + targetPosition.y;
        float bottomY = targetPosition.y - veritcalHalf;

        if (topY > screenTop)
        {
            targetPosition.y = screenTop - veritcalHalf - offset.y;
        }
        else if (bottomY < screenBottom)
        {
            targetPosition.y = screenBottom + veritcalHalf + offset.y;
        }
        rect.position = targetPosition;
    }

}
