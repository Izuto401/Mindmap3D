using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float moveSpeed = 10f; // カメラの移動速度
    public float lookSpeed = 2f; // カメラの回転速度

    private float rotationX = 0f;
    private float rotationY = 0f;

    void Update()
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