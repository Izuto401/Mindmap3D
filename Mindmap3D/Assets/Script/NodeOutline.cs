using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ノードを選択中に表示するアウトラインの機能・ノードプレハブにアタッチ
public class NodeOutline : MonoBehaviour
{
    public GameObject outlineObject;

    void Start()
    {
        outlineObject.SetActive(false);
    }

    // カーソルがノード上に移動時
    void OnMouseEnter()
    {
        outlineObject.SetActive(true);
    }

    // カーソルがノード上から離れた時
    void OnMouseExit()
    {
        outlineObject.SetActive(false);
    }

    // クリック時
    void OnMouseDown()
    {
        outlineObject.SetActive(true);
    }

    // 外部からのアウトライン解除呼び出し用
    public void DeactivateOutline()
    {
        outlineObject.SetActive(false);
    }
}
