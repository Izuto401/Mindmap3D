using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

// ノードを編集するボタンの管理・UIボタンにアタッチ
public class UIManager : MonoBehaviour
{ 
    public GameObject nodePrefab;
    public Transform nodeParent;
    public Button addNodeButton;
    public Button removeNodeButton;
    public Button applyFontSizeButton;
    public TMP_InputField fontSizeInput; // フォントサイズを入力するためのフィールド

    private GameObject selectedNode;

    void Start()
    {
        addNodeButton.onClick.AddListener(AddNode);
        removeNodeButton.onClick.AddListener(RemoveNode);
        applyFontSizeButton.onClick.AddListener(OnApplyFontSize);
    }

    public void AddNode()
    {
        Vector3 randomPosition = new Vector3(Random.Range(-5f, 5f), Random.Range(-5f, 5f), Random.Range(-5f, 5f));
        GameObject newNode = Instantiate(nodePrefab, randomPosition, Quaternion.identity, nodeParent);
        selectedNode = newNode;
    }

    public void RemoveNode()
    {
        if (selectedNode != null)
        {
            Destroy(selectedNode);
        }
        else
        {
            Debug.Log("削除するノードが選択されていません");
        }
    }

    public void OnApplyFontSize()
    {
        if (float.TryParse(fontSizeInput.text, out float newSize))
        {
            EditNodeFontSize(newSize);
        }
        else
        {
            Debug.Log("無効なフォントサイズが入力されました");
        }
    }

    public void EditNodeFontSize(float newSize)
    {
        if (selectedNode != null)
        {
            TMP_Text nodeText = selectedNode.GetComponentInChildren<TMP_Text>();
            if (nodeText != null)
            {
                nodeText.fontSize = newSize;
            }
        }
    }
}
