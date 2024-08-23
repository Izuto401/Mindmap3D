using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeEditButton : MonoBehaviour // ノードを編集するボタンの管理
{
    public GameObject nodePrefab;
    public Transform parentTransform;

    public void AddNode(Vector3 position) // ノード追加
    {
        Instantiate(nodePrefab, position, Quaternion.identity, parentTransform);
    }

    public void DeleteNode(GameObject node) // ノード削除
    {
        Destroy(node);
    }
}
