using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class NodeManager : MonoBehaviour
{
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

    // メインノードと選択されたノードの参照
    private GameObject mainNode;
    private GameObject selectedNode;

    // 編集モードのフラグ
    private bool isEditMode = false;

    void Start()
    {
        // メインノードを初期化
        mainNode = Instantiate(nodePrefab, nodeContainer);
        nodes.Add(mainNode);

        // 初期状態では編集モードをオフに
        ToggleUIVisibility(false);
    }

    // 新しいノードを追加するメソッド
    public void AddNode()
    {
        Vector3 randomPosition = new Vector3(Random.Range(-5f, 5f), Random.Range(-5f, 5f), Random.Range(-5f, 5f));
        AddNode(randomPosition);
    }

    // 引数付きのAddNode関数（DataManager用）
    public GameObject AddNode(Vector3 position)
    {
        GameObject newNode = Instantiate(nodePrefab, nodeContainer);
        newNode.transform.position = position;
        nodes.Add(newNode);

        // 選択されているノードがある場合、リンクを作成
        if (selectedNode != null)
        {
            CreateLink(selectedNode, newNode);
        }

        return newNode;
    }

    // ノードを削除するメソッド
    public void RemoveNode()
    {
        if (selectedNode != null)
        {
            nodes.Remove(selectedNode);
            Destroy(selectedNode);
            selectedNode = null;
        }
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

    // ノード名を編集するメソッド
    public void EditNodeName()
    {
        if (selectedNode != null)
        {
            NodeData nodeData = selectedNode.GetComponent<NodeData>();
            nodeData.nodeName = nodeNameInputField.text;
            outputMessage.text = "ノード名を更新しました。";
        }
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
