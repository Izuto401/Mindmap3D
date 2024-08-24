using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// マインドマップの生成統括・シーン内の空オブジェクトにアタッチ
public class MindmapManager : MonoBehaviour
{
    public GameObject initialNode; // 初期ノード
    public GameObject nodePrefab;
    public LinkManager linkManager;  // リンクを管理するスクリプトへの参照
    public Transform nodesParent;    // ノードをまとめて格納する親オブジェクト

    private List<GameObject> nodes = new List<GameObject>();

    void Start()
    {
        // 初期ノードをリストに追加
        nodes.Add(initialNode);
    }

    public void EditNode(GameObject node, Color newColor, int newFontSize)
    {
        UIManager nodeEditButton = node.GetComponent<UIManager>();
        if (nodeEditButton != null)
        {
            nodeEditButton.EditNodeFontSize(newFontSize);
        }
    }
}

