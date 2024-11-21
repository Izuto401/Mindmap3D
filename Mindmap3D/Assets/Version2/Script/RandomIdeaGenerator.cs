using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class RandomIdeaGenerator : MonoBehaviour
{
    public NodeManager nodeManager; // NodeManagerの参照
    public TextMeshProUGUI ideaOutputField; // アイディア出力用のテキストフィールド
    public TextMeshProUGUI numberDisplay; // 数字表示用のテキスト
    private int numberOfNodes = 3; // 初期値を3に設定
    private const int minNodes = 1;
    private const int maxNodes = 5;

    void Start()
    {
        if (numberDisplay == null)
        {
            Debug.LogError("numberDisplay is not assigned.");
        }
        if (ideaOutputField == null)
        {
            Debug.LogError("ideaOutputField is not assigned.");
        }
        if (nodeManager == null)
        {
            Debug.LogError("nodeManager is not assigned.");
        }

        UpdateNumberDisplay();
    }

    // 数字を増加させるメソッド
    public void IncrementNumber()
    {
        if (numberOfNodes < maxNodes)
        {
            numberOfNodes++;
            UpdateNumberDisplay();
        }
    }

    // 数字を減少させるメソッド
    public void DecrementNumber()
    {
        if (numberOfNodes > minNodes)
        {
            numberOfNodes--;
            UpdateNumberDisplay();
        }
    }

    // ランダムなアイディアを生成するメソッド
    public void GenerateRandomIdea()
    {
        List<GameObject> nodes = nodeManager.Nodes;
        if (nodes.Count < numberOfNodes)
        {
            ideaOutputField.text = "ノードの数が足りません。";
            return;
        }

        List<string> selectedNodeNames = new List<string>();
        HashSet<int> usedIndices = new HashSet<int>();

        while (selectedNodeNames.Count < numberOfNodes)
        {
            int randomIndex = Random.Range(0, nodes.Count);
            if (!usedIndices.Contains(randomIndex))
            {
                usedIndices.Add(randomIndex);
                NodeData nodeData = nodes[randomIndex].GetComponent<NodeData>();
                if (nodeData != null)
                {
                    selectedNodeNames.Add(nodeData.nodeName);
                }
            }
        }

        ideaOutputField.text = string.Join("＊", selectedNodeNames);
    }

    // 数字表示を更新するメソッド
    private void UpdateNumberDisplay()
    {
        numberDisplay.text = numberOfNodes.ToString();
    }
}
