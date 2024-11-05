using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContextNodeDisplay : MonoBehaviour
{
    public NodeManager nodeManager;
    public int numberOfChildNodesToShow = 3; // 表示する子ノードの数

    // コンテクストノードを表示するメソッド
    public void DisplayContext(GameObject selectedNode)
    {
        // 親ノードを表示
        NodeData selectedNodeData = selectedNode.GetComponent<NodeData>();
        GameObject parentNode = FindParentNode(selectedNodeData.parentNodeId);

        if (parentNode != null)
        {
            // 親ノードの表示ロジック
            // ここに親ノードの表示方法を記述
        }

        // 子ノードを表示
        List<GameObject> childNodes = FindChildNodes(selectedNodeData.nodeId);
        for (int i = 0; i < Mathf.Min(numberOfChildNodesToShow, childNodes.Count); i++)
        {
            // 子ノードの表示ロジック
            // ここに子ノードの表示方法を記述
        }
    }

    // 親ノードを検索するメソッド
    private GameObject FindParentNode(int parentNodeId)
    {
        foreach (GameObject node in nodeManager.Nodes)
        {
            if (node.GetComponent<NodeData>().nodeId == parentNodeId)
            {
                return node;
            }
        }
        return null;
    }

    // 子ノードを検索するメソッド
    private List<GameObject> FindChildNodes(int parentNodeId)
    {
        List<GameObject> childNodes = new List<GameObject>();
        foreach (GameObject node in nodeManager.Nodes)
        {
            if (node.GetComponent<NodeData>().parentNodeId == parentNodeId)
            {
                childNodes.Add(node);
            }
        }
        return childNodes;
    }
}
