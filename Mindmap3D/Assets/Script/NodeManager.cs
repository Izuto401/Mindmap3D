using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro; // TextMeshProを使用するためのライブラリ
using UnityEngine.UI; // UI要素を使用するためのライブラリ
using UnityEngine.EventSystems;

public class NodeManager : MonoBehaviour, IPointerClickHandler
{
    public int id; // ノードのID（識別用）
    public Vector3 position; // ノードの現在の位置
    public string nodeName; // ノードの名前（表示されるテキスト）
    public List<LinkManager> links = new List<LinkManager>(); // このノードに接続されているリンクのリスト

    private TextMeshPro textMesh; // ノード上に表示されるテキストのコンポーネント
    private Image nodeBackground; // ノードの背景画像（色を変更するために使用）
    private Color defaultColor = Color.white; // デフォルトの色
    private Color selectedColor = Color.green; // 選択されたときの色

    private bool isSelected = false; // ノードが選択されているかどうかを示すフラグ
    private TMP_InputField inputField; // ノードの名前を編集するための入力フィールド
    public Image outlineImage; // Outline画像用の変数

    void Start()
    {
        // MeshColliderがアタッチされているか確認し、なければ追加
        MeshCollider meshCollider = GetComponent<MeshCollider>();
        if (meshCollider == null)
        {
            meshCollider = gameObject.AddComponent<MeshCollider>();
            meshCollider.convex = true; // ColliderをConvexに設定する（必要に応じて）
        }

        // NodeBackgroundのImageコンポーネントを取得
        nodeBackground = transform.Find("NodeBackground").GetComponent<Image>();
        if (nodeBackground == null)
        {
            Debug.LogError("NodeBackground Image component is missing on " + gameObject.name);
        }

        // 子オブジェクトにあるTextMeshProコンポーネントを取得
        textMesh = GetComponentInChildren<TextMeshPro>();
        if (textMesh != null)
        {
            textMesh.text = nodeName; // 初期状態でノードの名前を表示
        }

        // 子オブジェクトにあるTMP_InputFieldコンポーネントを取得
        inputField = GetComponentInChildren<TMP_InputField>();
        if (inputField != null)
        {
            inputField.text = nodeName; // 初期状態でInputFieldにノード名を設定
            inputField.gameObject.SetActive(false); // 初期は非表示にする
            inputField.onEndEdit.AddListener(OnEndEdit); // 編集終了時の処理を設定
        }

        // Outline画像を探す（プレハブ内に配置されていることが前提）
        outlineImage = transform.Find("OutlineImage").GetComponent<Image>();
        if (outlineImage != null)
        {
            outlineImage.gameObject.SetActive(false); // 初期状態で非表示にする
        }

        transform.position = position;
        nodeBackground = GetComponentInChildren<Image>(); // NodeBackgroundイメージを取得
        SetDefaultColor();
    }

    void Update()
    {
        // ノードが選択された状態かどうかに応じて背景色を変更
        if (nodeBackground != null)
        {
            nodeBackground.color = isSelected ? Color.green : Color.white; // 選択された場合は緑色、そうでない場合は白色
        }

        // ノードの位置を常に更新（他のスクリプトで位置が変更された場合に対応）
        position = transform.position;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        // ダブルクリックを検知
        if (eventData.clickCount == 2 && inputField != null)
        {
            inputField.gameObject.SetActive(true);
            inputField.Select();
            inputField.ActivateInputField();
        }
    }

    private void OnMouseDown()
    {
        // 既に選択されている場合は選択を解除
        if (isSelected)
        {
            Deselect();
        }
        else
        {
            // 他のノードが選択されている場合は選択解除
            if (MindmapManager.Instance.selectedNode != null)
            {
                MindmapManager.Instance.selectedNode.Deselect();
            }

            // ノードを選択状態にする
            isSelected = true;

            // ノード選択時にOutline画像を有効化
            if (outlineImage != null)
            {
                outlineImage.gameObject.SetActive(true);
            }

            // MindmapManagerに通知して、このノードを選択されたノードとして登録
            MindmapManager.Instance.SelectNode(this);

            if (inputField != null)
            {
                inputField.gameObject.SetActive(false); // 選択時はInputFieldは非表示にする
            }
        }
    }

    private void OnEndEdit(string newText)
    {
        // 新しいテキストが空でない場合、ノード名を更新
        if (!string.IsNullOrEmpty(newText))
        {
            nodeName = newText; // ノード名を更新
            if (textMesh != null)
            {
                textMesh.text = nodeName; // TextMeshProに新しいノード名を表示
            }
        }

        // ノードの選択を解除
        Deselect();
    }

    // このノードにリンクを追加するメソッド
    public void AddLink(LinkManager link)
    {
        // リンクが既に追加されていない場合のみリストに追加
        if (!links.Contains(link))
        {
            links.Add(link);
        }
    }

    public void SetDefaultColor()
    {
        if (nodeBackground != null)
        {
            nodeBackground.color = defaultColor;
        }
    }

    public void Select()
    {
        if (nodeBackground != null)
        {
            nodeBackground.color = selectedColor; // 選択色に変更
        }
        Debug.Log("select!");
    }

    // ノードの選択を解除するメソッド
    public void Deselect()
    {
        isSelected = false;

        if (outlineImage != null)
        {
            outlineImage.gameObject.SetActive(false); // ノードの選択が解除されたらOutline画像を非表示にする
        }

        // テキスト編集用のInputFieldを非表示にする
        if (inputField != null)
        {
            inputField.gameObject.SetActive(false);
        }

        SetDefaultColor();
        Debug.Log("Deselect!");
    }
}
