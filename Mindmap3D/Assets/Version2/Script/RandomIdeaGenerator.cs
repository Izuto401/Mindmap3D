using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomIdeaGenerator : MonoBehaviour
{
    public NodeManager nodeManager;
    public int numberOfRandomNodes = 3; // 表示するランダムノードの数

    // ランダムなアイデアを生成するメソッド
    public void GenerateRandomIdeas()
    {
        // ノードのリストを取得
        List<GameObject> nodes = nodeManager.Nodes;
        if (nodes.Count == 0) return;

        // ランダムノードを選択
        HashSet<GameObject> randomNodes = new HashSet<GameObject>();
        while (randomNodes.Count < numberOfRandomNodes)
        {
            int randomIndex = Random.Range(0, nodes.Count);
            randomNodes.Add(nodes[randomIndex]);
        }

        // ランダムノードの表示ロジック
        foreach (var node in randomNodes)
        {
            // ランダムノードの表示方法を記述
            // 例えば、ランダムノードを特定のUIに表示する
            Debug.Log("Random Node: " + node.GetComponent<NodeData>().nodeName);
        }
    }
}
