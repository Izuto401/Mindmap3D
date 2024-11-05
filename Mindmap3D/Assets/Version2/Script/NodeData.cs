using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

// スクリプタブルオブジェクトとして作成
[CreateAssetMenu(fileName = "NodeData", menuName = "ScriptableObjects/NodeData", order = 1)]
public class NodeData : ScriptableObject
{
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
}
