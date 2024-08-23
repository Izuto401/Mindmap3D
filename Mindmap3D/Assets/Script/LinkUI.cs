using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// リンクに関するUI操作・UIボタンにアタッチ
public class LinkUI : MonoBehaviour
{
    public LinkManager linkManager; // 別スクリプト呼び出し
    public Button createLinkButton;
    public Button removeLinkButton;
    public GameObject[] selectedNodes;

    void Start()
    {
        createLinkButton.onClick.AddListener(() => linkManager.CreateLink(selectedNodes[0], selectedNodes[1]));
        removeLinkButton.onClick.AddListener(RemoveSelectedLink);
    }

    // リンク削除ボタンが押されたときの処理
    private void RemoveSelectedLink()
    {
        // 選択されたリンクを削除する処理をここに記述
    }
}
