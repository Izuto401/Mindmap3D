using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeEditButton : MonoBehaviour // ノードを編集するボタンの管理
{
    public GameObject nodePrefab;
    public Transform parentTransform;

    // ノード追加
    public void AddNode(Vector3 position)
    {
        Instantiate(nodePrefab, position, Quaternion.identity, parentTransform);
    }

    // ノード削除
    public void DeleteNode(GameObject node) 
    {
        Destroy(node);
    }
}
