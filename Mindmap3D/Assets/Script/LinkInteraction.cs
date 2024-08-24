using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// リンクの選択や操作・呼び出し用？（現時点8月23日時点）
public class LinkInteraction : MonoBehaviour
{
    public LinkManager linkManager; // 別スクリプト呼び出し

    // 複数のノードを選択しリンクを作成
    public void CreateLinkBetweenSelectedNodes(GameObject[] selectedNodes)
    {
        if (selectedNodes.Length < 2) return;

        for (int i = 0; i < selectedNodes.Length - 1; i++)
        {
            linkManager.CreateLink(selectedNodes[i], selectedNodes[i + 1]);
        }
    }

    // リンクの選択（選択時に何かを行いたい場合に追加）
    public void SelectLink(GameObject link)
    {
        // 選択されたリンクに対しての処理を記述、そのうちInputFieldつける予定
    }
}
