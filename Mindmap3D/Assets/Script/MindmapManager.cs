using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

/// <summary>
/// マインドマップ全体の管理を行うスクリプト。
/// ノードの追加・削除や、選択されたノードの管理を行います。
/// </summary>
public class MindmapManager : MonoBehaviour
{
    // シングルトンインスタンス
    public static MindmapManager Instance { get; private set; }

    public GameObject nodePrefab; // ノードのプレハブを保持
    public GameObject linkPrefab; // リンクのプレハブを保持
    public TextMeshProUGUI warningMessage; // 削除不可メッセージを表示するTextMeshProUGUIコンポーネント
    public NodeManager initialNode; // 初期表示のノード（削除不可に設定）
    public NodeManager selectedNode; // 現在選択されているノードを保持
    private Vector3 nodePosition;
    public NodeManager isSelected;
    public TextMeshProUGUI editingModeText; // 文字編集モードを表示するUI

    // シングルトンインスタンスの初期化
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    void Start()
    {
        // シーン上に存在する初期ノードを検索して参照する
        initialNode = FindObjectOfType<NodeManager>();

        if (initialNode != null)
        {
            initialNode.id = 1; // 必要に応じてIDを設定
            initialNode.nodeName = "Initial Node"; // 必要に応じて名前を設定
        }
        else
        {
            Debug.LogError("初期ノードがシーン上に見つかりませんでした。");
        }
    }

    // ノードを選択するメソッド
    public void SelectNode(NodeManager node)
    {
        if (selectedNode != null)
        {
            // 同じノードが再選択された場合
            if (selectedNode == node)
            {
                selectedNode.Deselect();
                selectedNode = null; // 選択解除
                CameraController.Instance.ResetNodePosition(); // カメラに選択解除を通知
                return; // ここで終了
            }

            // 別のノードを選択しようとしている場合
            selectedNode.Deselect(); // 以前の選択を解除
            CameraController.Instance.ResetNodePosition(); // カメラに選択解除を通知
        }

        selectedNode = node;
        selectedNode.Select(); // 新しいノードを選択
        nodePosition = node.transform.position; // ノードの位置を取得
    }

    public void DeselectNode()
    {
        if (selectedNode != null)
        {
            selectedNode.Deselect();
            selectedNode = null;
            CameraController.Instance.ResetNodePosition(); // カメラに選択解除を通知

            // 編集モードの表示を非表示にする
            if (editingModeText != null)
            {
                editingModeText.gameObject.SetActive(false);
            }
        }
    }

    // ボタンに設定するノード追加メソッド
    public void AddNode()
    {
        if (selectedNode != null)
        {
            Vector3 randomPosition = selectedNode.transform.position + new Vector3(Random.Range(-5f, 5f), Random.Range(-5f, 5f), 0);
            GameObject newNode = Instantiate(nodePrefab, randomPosition, Quaternion.identity);
            NodeManager newNodeScript = newNode.GetComponent<NodeManager>();
            newNodeScript.nodeName = "New Node";
            newNodeScript.position = randomPosition;

            // リンクを作成
            GameObject link = Instantiate(linkPrefab);
            LinkManager linkScript = link.GetComponent<LinkManager>();
            linkScript.nodeA = selectedNode;
            linkScript.nodeB = newNodeScript;
            selectedNode.AddLink(linkScript);
            newNodeScript.AddLink(linkScript);
        }
    }


    // ボタンに設定するノード削除メソッド
    public void RemoveNode()
    {
        // 初期ノードが選択されている場合、削除不可の処理を行う
        if (selectedNode == initialNode)
        {
            Debug.LogWarning("This node cannot be deleted."); // コンソールに警告メッセージを表示

            // 削除不可メッセージをUIに表示
            if (warningMessage != null)
            {
                warningMessage.text = "このノードは消去できません"; // メッセージを設定
                warningMessage.gameObject.SetActive(true); // メッセージを表示
                StartCoroutine(HideWarningMessage()); // 一定時間後にメッセージを非表示にするコルーチンを開始
            }
            return; // 処理を終了し、ノードの削除を行わない
        }

        // 選択されているノードがある場合
        if (selectedNode != null)
        {
            // ノードに関連する全てのリンクを削除
            foreach (var link in selectedNode.links)
            {
                if (link != null) // リンクが有効か確認
                {
                    Destroy(link.gameObject); // リンクを削除
                }
            }

            // ノード自体を削除
            Destroy(selectedNode.gameObject);
            selectedNode = null; // 選択ノードをリセット
        }
    }

    // 文字編集モードの表示を制御するメソッド
    public void ShowEditingMode(bool show)
    {
        if (editingModeText != null)
        {
            editingModeText.gameObject.SetActive(show);
            if (show)
            {
                editingModeText.text = "文字編集モード";
            }
        }
    }

    // 削除不可メッセージを一定時間表示し、その後非表示にするコルーチン
    IEnumerator HideWarningMessage()
    {
        yield return new WaitForSeconds(2); // 2秒間待機
        warningMessage.gameObject.SetActive(false); // メッセージを非表示
    }
}
