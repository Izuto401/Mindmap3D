using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    // カメラの回転速度
    public float rotateSpeed = 5f;

    // カメラのズーム速度
    public float zoomSpeed = 5f;

    // カメラが注視するターゲット
    public Transform target;

    void Update()
    {
        // 右クリックでカメラを回転
        if (Input.GetMouseButton(1))
        {
            float h = rotateSpeed * Input.GetAxis("Mouse X");
            float v = rotateSpeed * Input.GetAxis("Mouse Y");
            transform.RotateAround(target.position, Vector3.up, h);
            transform.RotateAround(target.position, transform.right, -v);
        }

        // スクロールでカメラをズーム
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        transform.Translate(0, 0, scroll * zoomSpeed, Space.Self);
    }
}
