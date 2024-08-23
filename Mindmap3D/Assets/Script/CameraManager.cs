using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour // カメラの基本管理機能
{
    public float moveSpeed = 10f; 
    public float lookSpeed = 2f;

    private float rotationX = 0f;
    private float rotationY = 0f;

    void Update()
    {
        // 移動関係
        float moveX = Input.GetAxis("Horizontal") * moveSpeed * Time.deltaTime;
        float moveZ = Input.GetAxis("Vertical") * moveSpeed * Time.deltaTime;
        transform.Translate(moveX, 0, moveZ);

        // 回転関係
        if (Input.GetMouseButton(1)) // 右クリックが押されている間
        {
            rotationX += Input.GetAxis("Mouse X") * lookSpeed;
            rotationY -= Input.GetAxis("Mouse Y") * lookSpeed;
            rotationY = Mathf.Clamp(rotationY, -90f, 90f); // カメラの上下回転を制限
            transform.localEulerAngles = new Vector3(rotationY, rotationX, 0);
        }
    }
}
