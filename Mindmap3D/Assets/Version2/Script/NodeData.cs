using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class NodeData : MonoBehaviour
{
    void Start()
    {
        creationDate = DateTime.Now;
        updateDate = DateTime.Now;
    }

    // ノードの識別子
    public int nodeId;

    // ノードの名前
    public string nodeName;

    // ノードの作成日
    public DateTime creationDate;

    // ノードの更新日
    public DateTime updateDate;

    // 親ノードの識別子
    public int parentNodeId;

    // ノードの位置
    public float positionX;
    public float positionY;
    public float positionZ;
}
