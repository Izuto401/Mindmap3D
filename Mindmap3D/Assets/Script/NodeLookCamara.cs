using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class  NodeLookCamara: MonoBehaviour
{
    private Camera mainCamera;

    void Start()
    {
        mainCamera = Camera.main;
    }

    void Update()
    {
        // カメラの方向を向くようにオブジェクトを回転させる
        if (mainCamera != null)
        {
            transform.LookAt(transform.position + mainCamera.transform.rotation * Vector3.forward,
                             mainCamera.transform.rotation * Vector3.up);
        }
    }
}
