using UnityEngine;

[System.Serializable]
public class ParallaxLayer//普通class，代表一层背景
                          //负责自己这层怎么移动、怎么循环
{
    [SerializeField] private Transform background;        // 这层的图片
    [SerializeField] private float parallaxMultiplier;   // 视差速度
    [SerializeField] private float imageWidthOffset = 0; // 提前触发循环的偏移

    private float imageFullWidth; // 图片完整宽度
    private float imageHalfWidth; // 图片一半宽度

    public void CalculateImageWidth()
    {
        // 从SpriteRenderer读取图片实际宽度
        imageFullWidth = background.GetComponent<SpriteRenderer>().bounds.size.x;
        imageHalfWidth = imageFullWidth / 2;
    }

    public void Move(float distanceToMove)
    {
        // 移动这层，乘以multiplier产生视差感
        background.position += Vector3.right * (distanceToMove * parallaxMultiplier);
    }

    public void LoopBackground(float cameraLeftEdge, float cameraRightEdge)
    {
        // 图片右边缘 和 左边缘
        float imageRightEdge = (background.position.x + imageHalfWidth) - imageWidthOffset;
        float imageLeftEdge = (background.position.x - imageHalfWidth) + imageWidthOffset;

        // 图片跑出摄像头左边 → 移到右边
        if (imageRightEdge < cameraLeftEdge)
            background.position += Vector3.right * imageFullWidth;

        // 图片跑出摄像头右边 → 移到左边
        else if (imageLeftEdge > cameraRightEdge)
            background.position += Vector3.right * -imageFullWidth;
    }
}