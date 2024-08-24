using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

// ノード編集・名前変更機能・ノードプレハブにアタッチ
public class NodeInputHandler : MonoBehaviour
{
    public TMP_InputField inputField;
    private bool isEditing = false;

    void Start()
    {
        inputField.gameObject.SetActive(false);
    }

    void Update()
    {
        if (isEditing && (Input.GetKeyDown(KeyCode.Return) || Input.GetMouseButtonDown(0)))
        {
            EndEditing();
        }
    }

    // クリック時のアクション
    void OnMouseDown()
    {
        if (!isEditing)
        {
            StartEditing();
        }
    }

    // 編集開始時の処理
    private void StartEditing()
    {
        inputField.gameObject.SetActive(true);
        inputField.ActivateInputField();
        isEditing = true;
    }

    // 編集終了時の処理
    private void EndEditing()
    {
        inputField.DeactivateInputField();
        inputField.gameObject.SetActive(false);
        isEditing = false;
    }
}
