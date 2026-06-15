using UnityEngine;

public class ParallaxBackground : MonoBehaviour//挂在GameObject上，负责整体管理
                                               //每帧计算摄像头移动了多少，通知每一层

{
    private Camera mainCamera;
    private float lastCameraPositionX; // 上一帧摄像头位置
    private float cameraHalfWidth;     // 摄像头一半宽度
    [SerializeField] private ParallaxLayer[] backgroundLayers; // 所有层

    private void Awake()
    {
        mainCamera = Camera.main;
        cameraHalfWidth = mainCamera.orthographicSize * mainCamera.aspect;
        lastCameraPositionX = mainCamera.transform.position.x;
        CalculateImageLength(); // 初始化每层图片宽度
    }

    private void FixedUpdate()
    {
        float currentCameraPositionX = mainCamera.transform.position.x;

        // 这帧摄像头移动了多少
        float distanceToMove = currentCameraPositionX - lastCameraPositionX;
        lastCameraPositionX = currentCameraPositionX;

        // 摄像头左右边界
        float cameraLeftEdge = currentCameraPositionX - cameraHalfWidth;
        float cameraRightEdge = currentCameraPositionX + cameraHalfWidth;

        // 通知每一层移动和循环
        foreach (ParallaxLayer layer in backgroundLayers)
        {
            layer.Move(distanceToMove);
            layer.LoopBackground(cameraLeftEdge, cameraRightEdge);
        }
    }

    private void CalculateImageLength()
    {
        foreach (ParallaxLayer layer in backgroundLayers)
            layer.CalculateImageWidth();
    }
}