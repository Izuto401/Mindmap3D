using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CameraController : MonoBehaviour
{
    // カメラの回転速度
    public float rotateSpeed = 50f;

    // カメラのズーム速度
    public float zoomSpeed = 50f;

    // カメラの移動速度
    public float moveSpeed = 10f;

    // ノードマネージャーの参照
    private NodeManager nodeManager;

    // 選択されたノードの参照
    private Transform selectedNode;

    // ノード名入力フィールドの参照
    public TMP_InputField nodeNameInputField;

    void Start()
    {
        // NodeManagerのインスタンスを取得
        nodeManager = FindObjectOfType<NodeManager>();
    }

    void Update()
    {
        // 入力フィールドがフォーカスされている場合はカメラ操作を無視
        if (nodeNameInputField != null && nodeNameInputField.isFocused)
        {
            return;
        }

        // 選択されたノードの更新
        if (nodeManager.SelectedNodes.Count > 0)
        {
            selectedNode = nodeManager.SelectedNodes[0].transform;
        }
        else
        {
            selectedNode = null;
        }

        // WASDキーでカメラを移動
        float horizontal = Input.GetAxis("Horizontal") * moveSpeed * Time.deltaTime; // A, Dキーで横移動
        float vertical = Input.GetAxis("Vertical") * moveSpeed * Time.deltaTime;     // W, Sキーで縦移動
        transform.Translate(horizontal, 0, vertical);

        // ノードを選択せずドラッグでカメラの角度変更
        if (Input.GetMouseButton(1) && selectedNode == null)
        {
            float h = rotateSpeed * Input.GetAxis("Mouse X");
            float v = rotateSpeed * Input.GetAxis("Mouse Y");
            transform.Rotate(v, h, 0);
        }

        // ドラッグで選択されたノードを中心にカメラを回転
        if (Input.GetMouseButton(1) && selectedNode != null)
        {
            float h = rotateSpeed * Input.GetAxis("Mouse X");
            float v = rotateSpeed * Input.GetAxis("Mouse Y");
            transform.RotateAround(selectedNode.position, Vector3.up, h);
            transform.RotateAround(selectedNode.position, transform.right, -v);
        }

        // スクロールでカメラをズーム
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if (selectedNode != null)
        {
            // 選択されたノードに近づく
            transform.position = Vector3.MoveTowards(transform.position, selectedNode.position, scroll * zoomSpeed);
        }
        else
        {
            // 何も選択されていない時はその場でズーム
            transform.Translate(0, 0, scroll * zoomSpeed, Space.Self);
        }
    }
}
