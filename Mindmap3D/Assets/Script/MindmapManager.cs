using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro; // TextMeshProを使用するために必要

public class MindmapManager : MonoBehaviour
{
    // シングルトンインスタンス
    public static MindmapManager Instance { get; private set; }

    public GameObject nodePrefab; // ノードのプレハブを保持
    public GameObject linkPrefab; // リンクのプレハブを保持
    public TextMeshProUGUI warningMessage; // 削除不可メッセージを表示するTextMeshProUGUIコンポーネント
    public NodeManager initialNode; // 初期表示のノード（削除不可に設定）

    public NodeManager selectedNode; // 現在選択されているノードを保持

    // シングルトンインスタンスの初期化
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this; // このスクリプトを実行しているオブジェクトをインスタンスとして設定
        }
        else if (Instance != this)
        {
            Destroy(gameObject); // 既にインスタンスが存在する場合、重複を避けるためにこのオブジェクトを破棄
        }
    }

    void Start()
    {
        // 初期ノードを生成し、位置を設定
        GameObject node1 = Instantiate(nodePrefab, new Vector3(0, 0, 0), Quaternion.identity);
        NodeManager nodeScript1 = node1.GetComponent<NodeManager>();
        nodeScript1.id = 1;
        nodeScript1.position = new Vector3(0, 0, 0);
        nodeScript1.nodeName = "Initial Node";
        initialNode = nodeScript1; // 初期ノードとして登録
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
                return; // ここで終了
            }

            // 別のノードを選択しようとしている場合
            selectedNode.Deselect(); // 以前の選択を解除
        }

        selectedNode = node;
        selectedNode.Select(); // 新しいノードを選択
    }


    // ボタンに設定するメソッド
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


    // ノードを削除するメソッド
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

    // 削除不可メッセージを一定時間表示し、その後非表示にするコルーチン
    IEnumerator HideWarningMessage()
    {
        yield return new WaitForSeconds(2); // 2秒間待機
        warningMessage.gameObject.SetActive(false); // メッセージを非表示
    }
}
