using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

/// <summary>
/// カメラの移動と回転を制御するスクリプト。
/// </summary>
public class CameraController : MonoBehaviour
{
    public static CameraController Instance;
    public float moveSpeed = 10f; // カメラの移動速度
    public float lookSpeed = 2f; // カメラの回転速度
    public float rotationSpeed = 50f; // ノードの周りを回転する速度

    private Vector3 nodePosition;
    private float rotationX;
    private float rotationY;
    public bool isNodeSelected = false;
    public bool isNodeMoveMode = false;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    private void Start()
    {
        // MindmapManagerのイベントにサブスクライブ
        if (MindmapManager.Instance != null)
        {
            MindmapManager.Instance.OnNodeMoveModeChanged += HandleNodeMoveModeChanged;
        }
    }

    private void HandleNodeMoveModeChanged(bool newNodeMoveMode)
    {
        isNodeMoveMode = newNodeMoveMode;
    }

    void Update()
    {
        // 現在のカメラの回転角度を取得
        Vector3 currentRotation = transform.localEulerAngles;
        rotationX = currentRotation.y;
        rotationY = currentRotation.x;


        // 文字編集中かどうかを確認
        if (MindmapManager.Instance.selectedNode != null)
        {
            TMP_InputField inputField = MindmapManager.Instance.selectedNode.GetComponentInChildren<TMP_InputField>();
            if (inputField != null && inputField.isFocused)
            {
                // 文字編集中はカメラの動きを停止
                return;
            }
        }
    }

    public void SetNodePosition(Vector3 position)
    {
        nodePosition = position;
        isNodeSelected = true;
        Debug.Log("isNodeSelected = true;");
    }

    public void ResetNodePosition()
    {
        isNodeSelected = false;
        Debug.Log("isNodeSelected = false;");
    }

    private void FixedUpdate()
    {
        if (isNodeMoveMode && isNodeSelected)
        {
            // ノード移動モードの操作
            Vector3 newPosition = nodePosition;

            // WASDキーでカメラから見た平面上の移動を制御
            float horizontal = Input.GetAxis("Horizontal");
            float vertical = Input.GetAxis("Vertical");

            Vector3 forward = transform.forward;
            Vector3 right = transform.right;

            // forwardとrightのY成分を無視して平面上の移動を計算
            forward.y = 0f;
            right.y = 0f;
            forward.Normalize();
            right.Normalize();

            Vector3 movement = (forward * vertical + right * horizontal) * moveSpeed * Time.deltaTime;
            newPosition += movement;

            // マウスホイールで奥行きを制御
            float scroll = Input.GetAxis("Mouse ScrollWheel");
            newPosition += transform.forward * scroll * moveSpeed;

            nodePosition = newPosition;
            MindmapManager.Instance.selectedNode.transform.position = newPosition;
        }
        else if (isNodeSelected)
        {
            // ノード選択時のカメラ操作（ノードの周りを回転）
            float horizontal = Input.GetAxis("Horizontal");
            float vertical = Input.GetAxis("Vertical");

            Vector3 direction = transform.position - nodePosition;
            Quaternion rotation = Quaternion.Euler(vertical * rotationSpeed, horizontal * rotationSpeed, 0);
            direction = rotation * direction;

            transform.position = nodePosition + direction;
            transform.LookAt(nodePosition);
        }
        else
        {
            // WASDキーでカメラの移動
            float moveX = Input.GetAxis("Horizontal") * moveSpeed * Time.deltaTime;
            float moveZ = Input.GetAxis("Vertical") * moveSpeed * Time.deltaTime;
            transform.Translate(moveX, 0, moveZ);

            // マウス/タッチパッドでカメラの回転
            if (Input.GetMouseButton(1)) // 右クリックが押されている間
            {
                rotationX += Input.GetAxis("Mouse X") * lookSpeed;
                rotationY -= Input.GetAxis("Mouse Y") * lookSpeed;
                //rotationY = Mathf.Clamp(rotationY, -90f, 90f); // カメラの上下回転を制限
                transform.localEulerAngles = new Vector3(rotationY, rotationX, 0);
            }
        }
    }
}