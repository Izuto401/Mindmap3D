using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    // ノード編集用の入力フィールド
    public InputField nodeNameInputField;

    // メッセージ出力欄
    public Text outputMessage;

    // NodeManagerの参照
    private NodeManager nodeManager;

    void Start()
    {
        nodeManager = FindObjectOfType<NodeManager>();
    }

    // ノード追加ボタンの処理
    public void AddNodeButton()
    {
        Vector3 randomPosition = new Vector3(Random.Range(-5f, 5f), Random.Range(-5f, 5f), Random.Range(-5f, 5f));
        nodeManager.AddNode(randomPosition);
    }

    // ノード削除ボタンの処理
    public void RemoveNodeButton()
    {
        if (nodeManager.SelectedNode != null)
        {
            nodeManager.RemoveNode(nodeManager.SelectedNode);
        }
    }

    // ノード名編集の処理
    public void EditNodeName()
    {
        if (nodeManager.SelectedNode != null)
        {
            NodeData nodeData = nodeManager.SelectedNode.GetComponent<NodeData>();
            nodeData.nodeName = nodeNameInputField.text;
            outputMessage.text = "ノード名を更新しました。";
        }
    }
}
