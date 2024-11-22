using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContextNodeDisplay : MonoBehaviour
{
    public NodeManager nodeManager;
    public GameObject contextNodeContainer;

    private GameObject selectedNode;
    private float distanceMultiplier = 200.0f; // ノード同士の距離を調整するための倍率
    private List<GameObject> allNodes = new List<GameObject>();
    private List<GameObject> allLinks = new List<GameObject>();
    private List<GameObject> contextNodes = new List<GameObject>();
    private List<GameObject> contextLinks = new List<GameObject>();

    private bool isContextDisplayed = false;

    void Start()
    {
        allNodes = nodeManager.Nodes; // すべてのノードを取得
        allLinks = nodeManager.Links; // すべてのリンクを取得
    }

    // ノード選択時にコンテクストを表示するメソッド
    public void DisplayContext(GameObject node)
    {
        if (isContextDisplayed)
        {
            Debug.LogWarning("Context is already displayed");
            return;
        }

        isContextDisplayed = true;
        Debug.Log("DisplayContext");
        selectedNode = node;
        ClearContextDisplay();

        // 既存の全ノードを非表示にする
        HideAllNodes();
        HideAllLinks();

        GameObject parentNode = nodeManager.GetParentNode(node);
        List<GameObject> childNodes = nodeManager.GetChildNodes(node);

        //Debug.Log($"選択ノード: {node.GetComponent<NodeData>().nodeName}");

        // 親ノードを表示
        if (parentNode != null)
        {
            DisplayNode(parentNode, new Vector3(0, 1 * distanceMultiplier, 0)); // 上に表示
            CreateLink(parentNode, node);
            //Debug.Log($"親ノード: {parentNode.GetComponent<NodeData>().nodeName}");
        }

        // 選択されたノードを表示
        DisplayNode(node, Vector3.zero); // 中央に表示

        // 子ノードを表示
        float angleStep = 360.0f / (childNodes.Count + 1);
        for (int i = 0; i < childNodes.Count; i++)
        {
            float angle = (i + 1) * angleStep;
            Vector3 childPosition = new Vector3(
                Mathf.Cos(angle * Mathf.Deg2Rad) * distanceMultiplier,
                -1 * distanceMultiplier,
                Mathf.Sin(angle * Mathf.Deg2Rad) * distanceMultiplier); // 下に円形に表示
            DisplayNode(childNodes[i], childPosition);
            CreateLink(node, childNodes[i]);
            //Debug.Log($"子ノード: {childNodes[i].GetComponent<NodeData>().nodeName}");
        }

        // コンテクストノードとリンクのリストを更新
        contextNodes.AddRange(childNodes);
        if (parentNode != null) contextNodes.Add(parentNode);
        contextNodes.Add(node);
    }

    // ノードを表示するメソッド
    private void DisplayNode(GameObject node, Vector3 position)
    {
        GameObject newNode = Instantiate(node, contextNodeContainer.transform);
        newNode.transform.localPosition = position;
        newNode.SetActive(true); // 表示
        contextNodes.Add(newNode); // 追加：コンテクストノードリストに追加
        //Debug.Log($"ノード表示: {node.GetComponent<NodeData>().nodeName} at {position}");
    }

    // リンクを作成するメソッド
    private void CreateLink(GameObject parent, GameObject child)
    {
        GameObject newLink = Instantiate(nodeManager.linkPrefab, contextNodeContainer.transform);
        LineRenderer lineRenderer = newLink.GetComponent<LineRenderer>();
        lineRenderer.SetPosition(0, parent.transform.localPosition);
        lineRenderer.SetPosition(1, child.transform.localPosition);
        contextLinks.Add(newLink); // コンテクストリンクリストに追加
        Debug.Log($"リンク作成: {parent.GetComponent<NodeData>().nodeName} -> {child.GetComponent<NodeData>().nodeName}");
    }

    // コンテクスト表示をクリアするメソッド
    private void ClearContextDisplay()
    {
        foreach (Transform child in contextNodeContainer.transform)
        {
            Destroy(child.gameObject);
        }
        contextNodes.Clear();
        contextLinks.Clear();
        Debug.Log("コンテクスト表示をクリア");
    }

    // すべてのノードを非表示にするメソッド
    private void HideAllNodes()
    {
        foreach (GameObject node in allNodes)
        {
            node.SetActive(false);
            Debug.Log($"HideAllNodes: ノード非表示: {node.GetComponent<NodeData>().nodeName}");
        }
    }

    // すべてのリンクを非表示にするメソッド
    private void HideAllLinks()
    {
        foreach (GameObject link in allLinks)
        {
            link.SetActive(false);
            Debug.Log($"HideAllLinks: リンク非表示: {link.name}");
        }
    }

    // コンテクストノード表示を終了し、元のノードとリンクを再表示するメソッド
    public void ResetContextDisplay()
    {
        ClearContextDisplay();
        ShowAllNodes();
        ShowAllLinks();
        isContextDisplayed = false; // フラグをリセット
        Debug.Log("ResetContextDisplay: すべてのノードとリンクを再表示");
    }

    private void ShowAllNodes()
    {
        foreach (GameObject node in allNodes)
        {
            node.SetActive(true);
            Debug.Log($"ShowAllNodes: ノード再表示: {node.GetComponent<NodeData>().nodeName}");
        }
    }

    private void ShowAllLinks()
    {
        foreach (GameObject link in allLinks)
        {
            link.SetActive(true);
            Debug.Log($"ShowAllLinks: リンク再表示: {link.name}");
        }
    }
}
