using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    // カメラの回転速度
    public float rotateSpeed = 5f;

    // カメラのズーム速度
    public float zoomSpeed = 5f;

    // ノードマネージャーの参照
    private NodeManager nodeManager;

    // 選択されたノードの参照
    private Transform selectedNode;

    void Start()
    {
        // NodeManagerのインスタンスを取得
        nodeManager = FindObjectOfType<NodeManager>();
    }

    void Update()
    {
        // 選択されたノードの更新
        selectedNode = nodeManager.SelectedNode?.transform;

        // スワイプでカメラの角度変更
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
