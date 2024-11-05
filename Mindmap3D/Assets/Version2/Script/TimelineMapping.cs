using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimelineMapping : MonoBehaviour
{
    public NodeManager nodeManager;
    public Transform timelineContainer;
    public GameObject timelineNodePrefab;

    // タイムラインノードを生成し、配置するメソッド
    public void DisplayTimeline()
    {
        // 既存のタイムラインノードを削除
        foreach (Transform child in timelineContainer)
        {
            Destroy(child.gameObject);
        }

        // ノードを作成日時でソート
        List<GameObject> nodes = nodeManager.Nodes;
        nodes.Sort((a, b) => a.GetComponent<NodeData>().creationDate.CompareTo(b.GetComponent<NodeData>().creationDate));

        // タイムライン上にノードを配置
        float spacing = 2.0f; // ノード間の距離
        for (int i = 0; i < nodes.Count; i++)
        {
            Vector3 position = new Vector3(i * spacing, 0, 0);
            GameObject timelineNode = Instantiate(timelineNodePrefab, position, Quaternion.identity, timelineContainer);
            timelineNode.GetComponent<NodeData>().nodeId = nodes[i].GetComponent<NodeData>().nodeId;
            timelineNode.GetComponentInChildren<TextMesh>().text = nodes[i].GetComponent<NodeData>().nodeName;
        }
    }
}
