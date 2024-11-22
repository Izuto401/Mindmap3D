using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ContextDisplayButton : MonoBehaviour
{
    public ContextNodeDisplay contextNodeDisplay; // ContextNodeDisplayスクリプトへの参照
    public NodeManager nodeManager; // NodeManagerスクリプトへの参照

    private void Start()
    {
        // ボタンのクリックイベントを設定
        Button button = GetComponent<Button>();
        button.onClick.AddListener(OnButtonClick);
    }

    public void OnButtonClick()
    {
        Debug.Log("Button clicked");

        // ノードが選択されているか確認
        if (nodeManager.SelectedNodes.Count > 0)
        {
            Debug.Log("ContextDisplayButton: Displaying context for selected node");
            // 最初の選択されたノードのコンテクストを表示
            contextNodeDisplay.DisplayContext(nodeManager.SelectedNodes[0]);
        }
        else
        {
            Debug.LogWarning("ノードが選択されていません。");
        }
    }
}
