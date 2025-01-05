using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NodeSelectionIndicator : MonoBehaviour
{
    public Image selectionIcon; // 選択アイコン（Inspector でアサイン）

    void Start()
    {
        if (selectionIcon != null)
        {
            selectionIcon.enabled = false; // 初期状態は非表示
        }
    }

    public void ShowIcon()
    {
        if (selectionIcon != null)
        {
            selectionIcon.enabled = true;
        }
    }

    public void HideIcon()
    {
        if (selectionIcon != null)
        {
            selectionIcon.enabled = false;
        }
    }
}

