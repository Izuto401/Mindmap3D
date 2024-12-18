using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class NodeManager : MonoBehaviour
{
    // NodeManager.csの冒頭にRandomIdeaGeneratorを参照する部分を追加
    public RandomIdeaGenerator randomIdeaGenerator;

    // ノードとリンクのプレハブを参照
    public GameObject nodePrefab;
    public GameObject linkPrefab;

    // ノードの親オブジェクト
    public Transform nodeParent;

    // ノード編集用の入力フィールド
    public TMP_InputField nodeNameInputField;

    // メッセージ出力欄
    public TextMeshProUGUI outputMessage;

    // 編集モードUIパネル
    public GameObject editModeUIPanel;

    // ノードとリンクを保持するコンテナ
    public Transform nodeContainer;
    public Transform linkContainer;

    // すべてのノードとリンクを保持するリスト
    private List<GameObject> nodes = new List<GameObject>();
    private List<GameObject> nodeLinks = new List<GameObject>();
    private List<GameObject> selectedNodes = new List<GameObject>(); // 複数選択されたノードを保持するリスト

    // メインノードと選択されたノードの参照
    private GameObject mainNode;
    private GameObject selectedNode;

    // 編集モードのフラグ
    private bool isEditMode = true;

    private int currentDepth = 0; // 現在の階層を管理
    private const float nodeDistanceIncrement = 300f; // 階層ごとの距離増分

    private Dictionary<GameObject, GameObject> parentMap = new Dictionary<GameObject, GameObject>(); // ノードの親子関係を管理する辞書
    private Dictionary<GameObject, List<GameObject>> childrenMap = new Dictionary<GameObject, List<GameObject>>(); // ノードの親子関係を管理する辞書


    // NodeManagerのStartメソッド内にRandomIdeaGeneratorの初期化を追加
    void Start()
    {
        // メインノードを初期化
        mainNode = Instantiate(nodePrefab, nodeContainer);
        nodes.Add(mainNode);

        // メインノードのRigidbodyを取得して固定
        Rigidbody mainNodeRb = mainNode.GetComponent<Rigidbody>();
        if (mainNodeRb != null)
        {
            mainNodeRb.isKinematic = true; // メインノードを固定
        }

        // 初期状態ではUIをonに
        ToggleUIVisibility(true);

        // RandomIdeaGeneratorの参照を設定
        randomIdeaGenerator.nodeManager = this;

        // ノードデータの設定
        NodeData mainNodeData = mainNode.GetComponent<NodeData>();
        mainNodeData.nodeName = "メインノード";
        mainNodeData.nodeId = 0; // 任意のID設定

        StartCoroutine(ResetNodeVelocities());
    }

    void Update()
    {
        // ノードクリック検出
        DetectNodeClick();

        // リンクの動的な更新
        UpdateLinks();
    }

    void UpdateLinks()
    {
        foreach (var link in nodeLinks)
        {
            LineRenderer lineRenderer = link.GetComponent<LineRenderer>();
            if (lineRenderer != null)
            {
                // リンクの両端のノードの位置を更新
                Vector3 startNodePosition = lineRenderer.GetPosition(0); // リンクの始点位置
                Vector3 endNodePosition = lineRenderer.GetPosition(1); // リンクの終点位置

                // ノードのリストから近いノードを検索
                GameObject startNode = FindNearestNode(startNodePosition);
                GameObject endNode = FindNearestNode(endNodePosition);

                if (startNode != null && endNode != null)
                {
                    lineRenderer.SetPosition(0, startNode.transform.position); // 親ノードの位置
                    lineRenderer.SetPosition(1, endNode.transform.position); // 子ノードの位置
                }
            }
        }
    }

    // 指定された位置に最も近いノードを検索するメソッド
    GameObject FindNearestNode(Vector3 position)
    {
        GameObject nearestNode = null;
        float minDistance = float.MaxValue;

        foreach (GameObject node in nodes)
        {
            float distance = Vector3.Distance(node.transform.position, position);
            if (distance < minDistance)
            {
                minDistance = distance;
                nearestNode = node;
            }
        }

        return nearestNode;
    }

    // 新しいノードを追加するメソッド
    public void AddNode()
    {
        // メインノードの位置を中心に、外側に発散するランダム位置を計算
        Vector3 randomPosition = GetPositionAroundMainNode();
        AddNode(randomPosition);
    }

    // 引数付きのAddNode関数（DataManager用）
    public GameObject AddNode(Vector3 position)
    {
        GameObject newNode = Instantiate(nodePrefab, nodeContainer);
        newNode.transform.position = position;
        nodes.Add(newNode);

        // ノードのRigidbody設定
        Rigidbody nodeRb = newNode.GetComponent<Rigidbody>();
        if (nodeRb != null)
        {
            nodeRb.velocity = Vector3.zero; // 初期速度をゼロに設定
            nodeRb.angularVelocity = Vector3.zero; // 初期角速度をゼロに設定
        }

        NodeData newNodeData = newNode.GetComponent<NodeData>();
        newNodeData.nodeName = "ノード" + nodes.Count; // 名前設定
        newNodeData.nodeId = nodes.Count; // 任意のID設定
        newNodeData.creationDate = DateTime.Now; // 生成時間の設定
        newNodeData.updateDate = DateTime.Now; // 更新時間の設定
        Debug.Log($"ノード追加: {newNodeData.nodeName} (ID: {newNodeData.nodeId})");


        // 選択されているノードがある場合、リンクを作成
        if (selectedNodes.Count > 0)
        {
            foreach (var selectedNode in selectedNodes)
            {
                CreateLink(selectedNode, newNode);
                SetParent(selectedNode, newNode); // 親子関係を設定
                Debug.Log($"リンク作成: 親 {selectedNode.GetComponent<NodeData>().nodeName} - 子 {newNodeData.nodeName}");
            }
        }
        else
        {
            // 新しいノードをメインノードにリンクする場合、階層を更新
            CreateLink(mainNode, newNode);
            SetParent(mainNode, newNode); // 親子関係を設定
            currentDepth++; // 新しい階層に進む
            Debug.Log($"リンク作成: 親 {mainNode.GetComponent<NodeData>().nodeName} - 子 {newNodeData.nodeName}");
        }

        return newNode;
    }

    // メインノードの周囲にランダムな位置を計算するメソッド (方向性を統一)
    private Vector3 GetPositionAroundMainNode()
    {
        // 現在の階層の半径を計算
        float radius = (currentDepth + 1) * nodeDistanceIncrement;

        // ランダムな方向を取得 (2D平面の場合はY軸を固定)
        float angle = UnityEngine.Random.Range(0f, 360f);
        float x = Mathf.Cos(angle * Mathf.Deg2Rad) * radius;
        float y = Mathf.Sin(angle * Mathf.Deg2Rad) * radius;
        float z = Mathf.Tan(angle * Mathf.Deg2Rad) * radius;

        // メインノードの位置を基準に計算
        Vector3 mainNodePosition = mainNode.transform.position;
        Vector3 randomPosition = mainNodePosition + new Vector3(x, y, z);

        return randomPosition;
    }


    // ノードを削除するメソッド
    public void RemoveNode()
    {
        // 削除対象のリストを保持
        List<GameObject> nodesToRemove = new List<GameObject>();

        foreach (var node in selectedNodes)
        {
            if (node == mainNode)
            {
                // メインノードの場合は削除せずメッセージを表示
                outputMessage.text = "メインノードは削除できません。";
                Debug.Log("Attempted to delete the main node, which is not allowed.");
                continue;
            }

            nodesToRemove.Add(node);
        }

        // 削除処理
        foreach (var node in nodesToRemove)
        {
            nodes.Remove(node);
            RemoveNodeLinks(node);
            Destroy(node);
        }

        selectedNodes.Clear();
        UpdateOutputMessage();
    }

    // ノードの関連リンクを削除するメソッド
    private void RemoveNodeLinks(GameObject node)
    {
        List<GameObject> linksToRemove = new List<GameObject>();

        foreach (var link in nodeLinks)
        {
            LineRenderer lineRenderer = link.GetComponent<LineRenderer>();
            if (lineRenderer.GetPosition(0) == node.transform.position || lineRenderer.GetPosition(1) == node.transform.position)
            {
                linksToRemove.Add(link);
            }
        }

        foreach (var link in linksToRemove)
        {
            nodeLinks.Remove(link);
            Destroy(link);
        }
    }

    // ノードを選択するメソッド
    public void SelectNode(GameObject node, bool isCtrlPressed)
    {
        if (isCtrlPressed)
        {
            if (selectedNodes.Contains(node))
            {
                selectedNodes.Remove(node);
            }
            else
            {
                selectedNodes.Add(node);
            }
        }
        else
        {
            if (selectedNodes.Contains(node))
            {
                selectedNodes.Remove(node);
            }
            else
            {
                selectedNodes.Clear();
                selectedNodes.Add(node);
            }
        }

        // 選択されたノードが一つの場合、その名前を入力フィールドに設定
        if (selectedNodes.Count == 1)
        {
            NodeData nodeData = selectedNodes[0].GetComponent<NodeData>();
            nodeNameInputField.text = nodeData.nodeName;
        }
        UpdateOutputMessage();
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

    // ノード名を編集するメソッド
    public void EditNodeName()
    {
        string newName = nodeNameInputField.text;
        Debug.Log("New name from input field: " + newName); // デバッグ用ログ

        foreach (var node in selectedNodes)
        {
            NodeData nodeData = node.GetComponent<NodeData>();
            nodeData.nodeName = newName;
            nodeData.updateDate = DateTime.Now; // 更新時間の設定
            Debug.Log("Node updated: " + nodeData.nodeName); // デバッグ用ログ

            // ノードに関連付けられたテキスト表示コンポーネントも更新する
            TextMeshProUGUI nodeText = node.GetComponentInChildren<TextMeshProUGUI>();
            if (nodeText != null)
            {
                nodeText.text = newName;
            }
        }
        outputMessage.text = "選択されたノードの名前を更新しました。";
        UpdateOutputMessage();
    }

    // 編集モードの切り替え
    public void SwitchEditMode()
    {
        isEditMode = !isEditMode;
        ToggleUIVisibility(isEditMode);
        outputMessage.text = isEditMode ? "編集モード: オン" : "編集モード: オフ";
    }

    // UIの表示/非表示を切り替える（引数なし）
    public void ToggleUIVisibility()
    {
        isEditMode = !isEditMode;
        editModeUIPanel.SetActive(isEditMode);
    }

    // UIの表示/非表示を切り替える（引数あり）
    private void ToggleUIVisibility(bool isVisible)
    {
        editModeUIPanel.SetActive(isVisible);
    }

    // ノードがクリックされたかを検出するメソッド
    private void DetectNodeClick()
    {
        if (Input.GetMouseButtonDown(0)) // 左クリック
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit))
            {
                // クリックされたオブジェクトがノードかどうか
                GameObject clickedNode = hit.transform.gameObject;

                if (clickedNode != null && nodes.Contains(clickedNode))
                {
                    bool isCtrlPressed = Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl);
                    SelectNode(clickedNode, isCtrlPressed);
                }
            }
        }
    }

    private void UpdateOutputMessage()
    {
        if (selectedNodes.Count == 0)
        {
            outputMessage.text = "選択されたノードはありません。";
        }
        else
        {
            outputMessage.text = "選択されたノード: " + string.Join(", ", selectedNodes.ConvertAll(node => node.GetComponent<NodeData>().nodeName));
        }
    }


    // 親子関係を設定するメソッド
    private void SetParent(GameObject parent, GameObject child)
    {
        if (!childrenMap.ContainsKey(parent))
        {
            childrenMap[parent] = new List<GameObject>();
        }
        childrenMap[parent].Add(child);
        parentMap[child] = parent;
    }

    // 指定されたノードの親ノードを取得するメソッド
    public GameObject GetParentNode(GameObject node)
    {
        return parentMap.ContainsKey(node) ? parentMap[node] : null;
    }

    // 指定されたノードの子ノードを取得するメソッド
    public List<GameObject> GetChildNodes(GameObject node)
    {
        return childrenMap.ContainsKey(node) ? childrenMap[node] : new List<GameObject>();
    }

    // すべてのノードを取得するプロパティ
    public List<GameObject> Nodes
    {
        get { return nodes; }
    }

    // 選択されているノードを取得するプロパティ
    public List<GameObject> SelectedNodes
    {
        get { return selectedNodes; }
    }

    // メインノードを取得するプロパティ
    public GameObject MainNode
    {
        get { return mainNode; }
    }

    // すべてのリンクを取得するプロパティ
    public List<GameObject> Links
    {
        get { return nodeLinks; }
    }

    private IEnumerator ResetNodeVelocities()
    {
        while (true)
        {
            yield return new WaitForSeconds(3f); // 1秒ごとにリセット

            foreach (GameObject node in nodes)
            {
                Rigidbody rb = node.GetComponent<Rigidbody>();
                if (rb != null && !rb.isKinematic)
                {
                    rb.velocity = Vector3.zero;
                    rb.angularVelocity = Vector3.zero;
                    Debug.Log("rb.velocity = Vector3.zero;");
                }
            }
        }
    }
}
