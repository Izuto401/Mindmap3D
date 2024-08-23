using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeManager : MonoBehaviour //ノードの基本管理機能
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

    public void Select() //ノード選択時
    {
        isSelected = true;
        UpdateColor();
    }
    public void DeSelect() //ノードの選択解除時
    {
        isSelected = false;
        UpdateColor();
    }

    public void SetDefaultColor() //ノードのデフォルト色
    {
        nodeRenderer.material.color = defaultColor;
    }

    private void UpdateColor() //ノードを選択時に色変更
    {
        nodeRenderer.material.color = isSelected ? selectedColor : defaultColor;
    }

    void OnMouseDown() //クリック時のアクション
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
