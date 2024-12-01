using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimelineMapping : MonoBehaviour
{
    public NodeManager nodeManager; // NodeManagerの参照
    public Button startTimelineMappingButton; // TimelineMappingモード開始ボタン
    public Button nextButton; // 次へボタン
    public Button backButton; // 戻るボタン
    public Button endTimelineMappingButton; // TimelineMappingモード終了ボタン

    private List<GameObject> sortedNodes; // 時系列順に並べられたノード
    public int currentIndex; // 現在のインデックス

    void Start()
    {
        nextButton.gameObject.SetActive(false); // 初期状態では非表示
        backButton.gameObject.SetActive(false); // 初期状態では非表示
        endTimelineMappingButton.gameObject.SetActive(false); // 初期状態では非表示
    }

    public void StartTimelineMapping()
    {
        // すべてのノードを非表示にし、メインノードのみ表示
        foreach (var node in nodeManager.Nodes)
        {
            node.SetActive(false);
        }
        nodeManager.MainNode.SetActive(true);

        // ノードを時系列順にソート
        sortedNodes = new List<GameObject>(nodeManager.Nodes);
        sortedNodes.Sort((a, b) => a.GetComponent<NodeData>().creationDate.CompareTo(b.GetComponent<NodeData>().creationDate));

        currentIndex = 0;
        nextButton.gameObject.SetActive(true);
        backButton.gameObject.SetActive(true);
        endTimelineMappingButton.gameObject.SetActive(true);
    }

    public void ShowNextNode()
    {
        Debug.Log("ShowNextNode");
        if (currentIndex < sortedNodes.Count) // 現在のインデックスがノード数未満であることを確認
        {
            sortedNodes[currentIndex].SetActive(true); // 現在のインデックスのノードを表示
            currentIndex++; // インデックスを進める
        }
    }

    public void HidePreviousNode()
    {
        if (currentIndex > 0) // 現在のインデックスが0より大きいことを確認
        {
            currentIndex--; // インデックスを戻す
            sortedNodes[currentIndex].SetActive(false); // 戻したインデックスのノードを非表示
        }
    }



    public void EndTimelineMapping()
    {
        // すべてのノードを再表示
        foreach (var node in nodeManager.Nodes)
        {
            node.SetActive(true);
        }

        nextButton.gameObject.SetActive(false);
        backButton.gameObject.SetActive(false);
        endTimelineMappingButton.gameObject.SetActive(false);
    }
}
