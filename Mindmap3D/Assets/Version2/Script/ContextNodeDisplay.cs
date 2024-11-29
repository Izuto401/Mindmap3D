using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContextNodeDisplay : MonoBehaviour
{
    public NodeManager nodeManager; // NodeManagerの参照
    public GameObject contextNodeContainer; // コンテクストノードを格納するコンテナ
    public GameObject linkPrefab; // リンクのプレハブ

    private GameObject selectedNode; // 選択されたノード
    private float distanceMultiplier = 200.0f; // ノード同士の距離を調整するための倍率
    private List<GameObject> allNodes = new List<GameObject>(); // すべてのノード
    private List<GameObject> allLinks = new List<GameObject>(); // すべてのリンク
    private List<GameObject> contextNodes = new List<GameObject>(); // コンテクストノード
    private List<GameObject> contextLinks = new List<GameObject>(); // コンテクストリンク

    private bool isContextDisplayed = false; // コンテクスト表示フラグ

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
        selectedNode = node;
        ClearContextDisplay();

        // 既存の全ノードを非表示にする
        HideAllNodes();
        HideAllLinks();

        GameObject parentNode = nodeManager.GetParentNode(node); // 親ノードを取得
        List<GameObject> childNodes = nodeManager.GetChildNodes(node); // 子ノードを取得

        // 親ノードを表示
        if (parentNode != null)
        {
            DisplayNode(parentNode, new Vector3(0, distanceMultiplier, 0)); // 上に表示
            contextLinks.Add(CreateLink(parentNode, node)); // リンクを作成してリストに追加
        }

        // 選択されたノードを表示
        DisplayNode(node, Vector3.zero); // 中央に表示

        // 子ノードを表示
        float spacing = distanceMultiplier; // 子ノード間の間隔を設定
        for (int i = 0; i < childNodes.Count; i++)
        {
            Vector3 childPosition = new Vector3(
                (i - (childNodes.Count - 1) / 2.0f) * spacing, // x軸に一定の間隔で配置
                -distanceMultiplier, // y軸に一定の距離
                0); // z軸は0に設定
            DisplayNode(childNodes[i], childPosition);
            contextLinks.Add(CreateLink(node, childNodes[i])); // リンクを作成してリストに追加
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
        newNode.transform.localPosition = position; // 指定された位置に配置
        newNode.SetActive(true); // 表示
        contextNodes.Add(newNode); // コンテクストノードリストに追加
    }

    // リンクを作成するメソッド
    private GameObject CreateLink(GameObject parent, GameObject child)
    {
        GameObject newLink = Instantiate(linkPrefab, contextNodeContainer.transform);
        LineRenderer lineRenderer = newLink.GetComponent<LineRenderer>();
        lineRenderer.positionCount = 2;
        lineRenderer.SetPosition(0, parent.transform.localPosition); // 親ノードの位置に設定
        lineRenderer.SetPosition(1, child.transform.localPosition); // 子ノードの位置に設定
        return newLink; // 作成したリンクを返す
    }

    // コンテクスト表示をクリアするメソッド
    private void ClearContextDisplay()
    {
        foreach (Transform child in contextNodeContainer.transform)
        {
            Destroy(child.gameObject); // コンテクストノードとリンクを削除
        }
        contextNodes.Clear(); // コンテクストノードリストをクリア
        contextLinks.Clear(); // コンテクストリンクリストをクリア
    }

    // すべてのノードを非表示にするメソッド
    private void HideAllNodes()
    {
        foreach (GameObject node in allNodes)
        {
            node.SetActive(false); // ノードを非表示
        }
    }

    // すべてのリンクを非表示にするメソッド
    private void HideAllLinks()
    {
        foreach (GameObject link in allLinks)
        {
            link.SetActive(false); // リンクを非表示
        }
    }

    // コンテクストノード表示を終了し、元のノードとリンクを再表示するメソッド
    public void ResetContextDisplay()
    {
        ClearContextDisplay();
        ShowAllNodes();
        ShowAllLinks();
        isContextDisplayed = false; // フラグをリセット
    }

    // すべてのノードを再表示するメソッド
    private void ShowAllNodes()
    {
        foreach (GameObject node in allNodes)
        {
            node.SetActive(true); // ノードを再表示
        }
    }

    // すべてのリンクを再表示するメソッド
    private void ShowAllLinks()
    {
        foreach (GameObject link in allLinks)
        {
            link.SetActive(true); // リンクを再表示
        }
    }
}
