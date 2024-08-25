using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ノード間のリンクを管理し、リンクの表示を制御するスクリプト。
/// </summary>
public class LinkManager : MonoBehaviour
{
    public NodeManager nodeA; // リンクの開始ノード
    public NodeManager nodeB; // リンクの終了ノード

    private LineRenderer lineRenderer;

    void Start()
    {
        // 既存の LineRenderer コンポーネントを取得
        lineRenderer = GetComponent<LineRenderer>();
        if (lineRenderer == null)
        {
            lineRenderer = gameObject.AddComponent<LineRenderer>();
        }
        lineRenderer.positionCount = 2; // リンクは2つのポイントを持つ
        lineRenderer.startWidth = 0.1f; // ラインの太さを設定
        lineRenderer.endWidth = 0.1f;

        // シンプルなマテリアルの設定
        lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
        lineRenderer.startColor = Color.white;
        lineRenderer.endColor = Color.white;
    }

    void Update()
    {
        if (nodeA != null && nodeB != null)
        {
            // ラインの位置をノードの位置に合わせて更新
            Vector3[] positions = new Vector3[2];
            positions[0] = nodeA.transform.position;
            positions[1] = nodeB.transform.position;
            lineRenderer.SetPositions(positions);
        }
    }
}
