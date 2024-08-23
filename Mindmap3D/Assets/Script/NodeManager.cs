using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeManager : MonoBehaviour // ノードの基本管理機能
{
    public bool isSelected = false;
    public Color defaultColor = Color.white;
    public Color selectedColor = Color.yellow;

    private Renderer nodeRenderer;

    void Start()
    {
        nodeRenderer = GetComponent<Renderer>();
        SetDefaultColor();
    }

    void Update()
    {
        UpdateColor();
    }

    // ノード選択時
    public void Select()
    {
        isSelected = true;
        UpdateColor();
    }

    // ノードの選択解除時
    public void DeSelect()
    {
        isSelected = false;
        UpdateColor();
    }

    // ノードのデフォルト色
    public void SetDefaultColor()
    {
        nodeRenderer.material.color = defaultColor;
    }

    // ノードを選択時に色変更
    private void UpdateColor()
    {
        nodeRenderer.material.color = isSelected ? selectedColor : defaultColor;
    }

    // クリック時のアクション
    void OnMouseDown()
    {
        if (!isSelected)
        {
            Select();
        }
        else
        {
            DeSelect();
        }
    }
}
