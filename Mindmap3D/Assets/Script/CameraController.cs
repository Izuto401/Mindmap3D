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
    private float rotationX = 0f;
    private float rotationY = 0f;
    private bool isNodeSelected = false;


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

    void Update()
    {
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

    private void LateUpdate()
    {
        if (isNodeSelected)
        {
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
                rotationY = Mathf.Clamp(rotationY, -90f, 90f); // カメラの上下回転を制限
                transform.localEulerAngles = new Vector3(rotationY, rotationX, 0);
            }
        }
    }
}