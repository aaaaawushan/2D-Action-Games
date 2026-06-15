using UnityEngine;
using System;
using UnityEngine.UI;
using System.Collections.Generic;
[Serializable]
public class UI_TreeConnectDetails//¹ÒÔÚ¸¸½ÚµãÉÏ£¬Í³Ò»¹ÜÀúçùÓÐÁ¬³öÈ¥µÄÏß
{
    public UI_TreeConnectHandler childNode;
    public NodeDirectionType direction;
    [Range(100f, 350f)] public float length;
    [Range(-25f, 25f)] public float rotation;
}

public class UI_TreeConnectHandler : MonoBehaviour
{
    private RectTransform rect;
    [SerializeField] private UI_TreeConnectDetails[] connectionDetails;
    [SerializeField] private UI_TreeConnection[] connections;


    private Image connectionImage;
    private Color originalColor;

    private bool _isUpdating = false;
 
    private void Awake()
    {
        rect = GetComponent<RectTransform>();
        if (connectionImage != null)
        {
            originalColor = connectionImage.color;
        }
    }
    public UI_TreeNode[] GetChildNode()
    {
        List<UI_TreeNode> childrenToReturn= new List<UI_TreeNode>();
        foreach(var node in connectionDetails)
        {
            if (node.childNode != null)
            {
                childrenToReturn.Add(node.childNode.GetComponent<UI_TreeNode>());
            }

        }
        return childrenToReturn.ToArray();
    }
    private void OnValidate()
    {
        if (connectionDetails.Length <= 0) return;
        if (connectionDetails.Length != connections.Length)
        {
            Debug.Log("Amount of details should be same as amount of connections. - " + gameObject.name);
            return;
        }

        // ±à¼­Æ÷ÏÂ Awake ²»ÔËÐÐ£¬ÐèÒªÔÚÕâÀE·±£ rect ÒÑ»º´E
        if (rect == null) rect = GetComponent<RectTransform>();

        UpdateConnections();
    }

    public void UpdateConnections()
    {
        for (int i = 0; i < connectionDetails.Length; i++)
        {
            var detail = connectionDetails[i];
            var connection = connections[i];

            Vector2 targetPosition = connection.GetConnectionPoint(rect);
            Image connectionImage = connection.GetConnectionImage();

            connection.DirectConnection(detail.direction, detail.length, detail.rotation);

            if (detail.childNode == null)
                continue;

            detail.childNode.SetPosition(targetPosition);
            detail.childNode.SetConnectionImage(connectionImage);
            //detail.childNode.transform.SetAsLastSibling();
        }
    }



    public void UpdateAllConnections()
    {
        if (_isUpdating) return;
        _isUpdating = true;

        UpdateConnections();
        foreach (var node in connectionDetails)
        {
            if (node.childNode == null) continue;
            node.childNode.UpdateAllConnections();
        }

        _isUpdating = false;
    }
    public void UnlockConnectionImage(bool unlocked)
    {
        if (connectionImage == null)
            return;

        connectionImage.color = unlocked ? Color.white : originalColor;
    }

    public void SetConnectionImage(Image image) => connectionImage = image;
    public void SetPosition(Vector2 position)
    {
        if (rect == null) rect = GetComponent<RectTransform>();
        if (rect == null) return;
        rect.anchoredPosition = position;
    }

}
