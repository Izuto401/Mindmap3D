using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LinkManager : MonoBehaviour // リンクの生成と管理
{
    public GameObject linkPrefab; 
    private List<GameObject> links = new List<GameObject>(); // 作成されたリンクのリスト

    // ノード間にリンクを作成
    public void CreateLink(GameObject nodeA, GameObject nodeB)
    {
        GameObject newLink = Instantiate(linkPrefab);
        links.Add(newLink);
        UpdateLink(newLink, nodeA.transform.position, nodeB.transform.position);
    }

    // リンクの位置と長さを更新
    public void UpdateLink(GameObject link, Vector3 start, Vector3 end)
    {
        link.transform.position = (start + end) / 2;
        link.transform.right = (end - start).normalized;
        link.transform.localScale = new Vector3(Vector3.Distance(start, end), 0.1f, 0.1f);
    }

    // 不要になったリンクの削除
    public void RemoveLink(GameObject link)
    {
        links.Remove(link);
        Destroy(link);
    }
}
