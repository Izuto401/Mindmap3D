using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeManager : MonoBehaviour
{
    // ノードとリンクのプレハブを参照
    public GameObject nodePrefab;
    public GameObject linkPrefab;

    // ノードとリンクを保持するコンテナ
    public Transform nodeContainer;
    public Transform linkContainer;

    // すべてのノードとリンクを保持するリスト
    private List<GameObject> nodes = new List<GameObject>();
    private List<GameObject> nodeLinks = new List<GameObject>();

    // メインノードと選択されたノードの参照
    private GameObject mainNode;
    private GameObject selectedNode;

    void Start()
    {
        // メインノードを初期化
        mainNode = Instantiate(nodePrefab, nodeContainer);
        nodes.Add(mainNode);
    }

    // 新しいノードを追加するメソッド
    public void AddNode()
    {
        GameObject newNode = Instantiate(nodePrefab, nodeContainer);
        newNode.transform.position = Random.insideUnitSphere * 5f; // ノードをランダムな位置に配置
        Nodes.Add(newNode);

        // 選択されているノードがある場合、リンクを作成
        if (selectedNode != null)
        {
            CreateLink(selectedNode, newNode);
        }
    }

    // 引数付きのAddNode関数
    public GameObject AddNode(Vector3 position)
    {
        GameObject newNode = Instantiate(nodePrefab, nodeContainer);
        newNode.transform.position = position;
        Nodes.Add(newNode);
        return newNode;
    }

    // ノードを削除するメソッド
    public void RemoveNode(GameObject node)
    {
        nodes.Remove(node);
        Destroy(node);
    }

    // ノードを選択するメソッド
    public void SelectNode(GameObject node)
    {
        selectedNode = node;
    }

    // ノード間のリンクを作成するメソッド
    private void CreateLink(GameObject parent, GameObject child)
    {
        GameObject newLink = Instantiate(linkPrefab, linkContainer);
        LineRenderer lineRenderer = newLink.GetComponent<LineRenderer>();
        lineRenderer.SetPosition(0, parent.transform.position);
        lineRenderer.SetPosition(1, child.transform.position);
        nodeLinks.Add(newLink);
    }

    // すべてのノードを取得するプロパティ
    public List<GameObject> Nodes
    {
        get { return nodes; }
    }

    // 選択されているノードを取得するプロパティ
    public GameObject SelectedNode
    {
        get { return selectedNode; }
    }
}
