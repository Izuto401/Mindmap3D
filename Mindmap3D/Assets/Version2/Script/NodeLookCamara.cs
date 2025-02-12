using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ノードが常にカメラの方向を向くようにするスクリプト。
/// </summary>
public class  NodeLookCamara: MonoBehaviour
{
    private Camera mainCamera;

    void Start()
    {
        // シーン内のメインカメラを取得
        mainCamera = Camera.main;
    }

    void Update()
    {
        // ノードの回転をカメラの方向に合わせる
        transform.LookAt(transform.position
            + mainCamera.transform.rotation
            * Vector3.forward,mainCamera.transform.rotation * Vector3.up);
    }
}
