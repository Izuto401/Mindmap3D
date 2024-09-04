using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;

/// <summary>
/// ノード間のリンクを管理し、リンクの表示を制御するスクリプト。
/// </summary>
public class LinkManager : MonoBehaviour, IPointerClickHandler
{
    public NodeManager nodeA; // リンクの開始ノード
    public NodeManager nodeB; // リンクの終了ノード
    public Canvas linkCanvas; // 関係性を示すCanvas
    public TextMeshProUGUI relationshipText; // 関係性を示すTextMeshPro
    public TMP_InputField relationshipInputField; // 関係性を編集するInputField

    private LineRenderer lineRenderer;

    void Start()
    {
        if (linkCanvas == null || relationshipText == null || relationshipInputField == null)
        {
            Debug.LogError("LinkManager: Canvas, TextMeshProUGUI, or TMP_InputField is not assigned.");
            return;
        }

        // 初期関係性を設定
        UpdateRelationshipUI();

        // InputFieldのイベント設定
        relationshipInputField.onEndEdit.AddListener(OnRelationshipEditEnd);

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

        // CanvasとInputFieldをリンクの中心に配置
        if (linkCanvas != null && nodeA != null && nodeB != null)
        {
            Vector3 middlePoint = (nodeA.transform.position + nodeB.transform.position) / 2;
            linkCanvas.transform.position = middlePoint;

            // Canvasが常にカメラに向くようにする
            //linkCanvas.transform.LookAt(Camera.main.transform);

            // InputFieldの位置をCanvasの中心に設定
            relationshipInputField.transform.position = linkCanvas.transform.position;
        }
    }

    // 関係性を設定するメソッド
    public void SetRelationship(string relationship)
    {
        if (relationshipText != null)
        {
            relationshipText.text = relationship;
        }
    }

    // 関係性に応じてUIを更新するメソッド
    void UpdateRelationshipUI()
    {
        // 例: ノードの名前を使って関係性を表示
        string relationship = $"{nodeA.nodeName} - {nodeB.nodeName}";
        SetRelationship(relationship);
    }

    // 編集モードに入るメソッド
    public void EnterEditMode()
    {
        if (relationshipText != null && relationshipInputField != null)
        {
            relationshipInputField.text = relationshipText.text;
            relationshipText.gameObject.SetActive(false);
            relationshipInputField.gameObject.SetActive(true);
            relationshipInputField.Select();
            relationshipInputField.ActivateInputField();
        }
    }

    // 編集完了時に呼ばれるメソッド
    public void OnRelationshipEditEnd(string newRelationship)
    {
        if (relationshipText != null && relationshipInputField != null)
        {
            SetRelationship(newRelationship);
            relationshipText.gameObject.SetActive(true);
        }
    }

    // IPointerClickHandlerを実装してダブルクリックを検出する
    private float lastClickTime;
    private const float doubleClickThreshold = 0.3f; // ダブルクリックとみなす時間間隔（秒）

    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log("Click detected!"); // デバッグ用ログ
        if (Time.time - lastClickTime < doubleClickThreshold)
        {
            Debug.Log("Double click detected!"); // ダブルクリック検出時のログ
            EnterEditMode();
        }
        lastClickTime = Time.time;
    }
}
