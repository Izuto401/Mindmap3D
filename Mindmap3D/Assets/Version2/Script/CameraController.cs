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

    // カメラの初期位置と回転
    private Vector3 initialPosition;
    private Quaternion initialRotation;

    void Start()
    {
        // NodeManagerのインスタンスを取得
        nodeManager = FindObjectOfType<NodeManager>();

        // カメラの初期位置と回転を保存
        initialPosition = transform.position;
        initialRotation = transform.rotation;
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

    // カメラの位置と回転を初期値にリセットするメソッド
    public void ResetCameraPosition()
    {
        transform.position = initialPosition;
        transform.rotation = initialRotation;
    }

    // ダブルクリックでカメラをノードの位置へ移動するメソッド
    public void MoveCameraToNode(Vector3 nodePosition)
    {
        // カメラの目標位置を設定（ノードの x, y から -500 の位置）
        Vector3 newCameraPosition = new Vector3(nodePosition.x, nodePosition.y, nodePosition.z - 500);

        // カメラの位置を移動
        transform.position = newCameraPosition;

        // カメラの角度をリセット
        transform.rotation = Quaternion.Euler(Vector3.zero);
    }
}
