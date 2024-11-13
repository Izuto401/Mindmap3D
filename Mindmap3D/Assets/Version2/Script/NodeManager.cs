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
    private List<GameObject> selectedNodes = new List<GameObject>(); // 複数選択されたノードを保持するリスト

    // メインノードと選択されたノードの参照
    private GameObject mainNode;
    private GameObject selectedNode;

    // 編集モードのフラグ
    private bool isEditMode = true;

    void Start()
    {
        // メインノードを初期化
        mainNode = Instantiate(nodePrefab, nodeContainer);
        nodes.Add(mainNode);

        // 初期状態ではUIをonに
        ToggleUIVisibility(true);
    }

    void Update()
    {
        // ノードクリック検出
        DetectNodeClick();
    }

    // 新しいノードを追加するメソッド
    public void AddNode()
    {
        Vector3 randomPosition = new Vector3(Random.Range(-100f, 100f), Random.Range(-100f, 100f), Random.Range(-1f, 1f));
        AddNode(randomPosition);
    }

    // 引数付きのAddNode関数（DataManager用）
    public GameObject AddNode(Vector3 position)
    {
        GameObject newNode = Instantiate(nodePrefab, nodeContainer);
        newNode.transform.position = position;
        nodes.Add(newNode);

        // 選択されているノードがある場合、リンクを作成
        if (selectedNodes.Count > 0)
        {
            foreach (var selectedNode in selectedNodes)
            {
                CreateLink(selectedNode, newNode);
            }
        }

        return newNode;
    }

    // ノードを削除するメソッド
    public void RemoveNode()
    {
        foreach (var node in selectedNodes)
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
        foreach (var node in selectedNodes)
        {
            NodeData nodeData = node.GetComponent<NodeData>();
            nodeData.nodeName = nodeNameInputField.text;
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
            outputMessage.text = "選択されたノード: " + string.Join(", ", selectedNodes.ConvertAll(node => node.name));
        }
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
}
