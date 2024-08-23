using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class NodeInputHandler : MonoBehaviour // ノード編集・名前変更機能
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

    void OnMouseDown() // クリック時のアクション
    {
        if (!isEditing)
        {
            StartEditing();
        }
    }

    private void StartEditing() // 編集開始時の処理
    {
        inputField.gameObject.SetActive(true);
        inputField.ActivateInputField();
        isEditing = true;
    }

    private void EndEditing() // 編集終了時の処理
    {
        inputField.DeactivateInputField();
        inputField.gameObject.SetActive(false);
        isEditing = false;
    }
}
