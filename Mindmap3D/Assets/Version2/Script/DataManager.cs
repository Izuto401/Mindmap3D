using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class DataManager : MonoBehaviour
{
    // NodeManagerの参照
    public NodeManager nodeManager;

    // データを保存するメソッド
    public void SaveData()
    {
        List<NodeData> nodeDataList = new List<NodeData>();
        foreach (GameObject node in nodeManager.Nodes)
        {
            NodeData data = node.GetComponent<NodeData>();
            nodeDataList.Add(data);
        }

        string json = JsonUtility.ToJson(nodeDataList);
        File.WriteAllText(Application.persistentDataPath + "/data.json", json);
    }

    // データをロードするメソッド
    public void LoadData()
    {
        string path = Application.persistentDataPath + "/data.json";
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            List<NodeData> nodeDataList = JsonUtility.FromJson<List<NodeData>>(json);

            foreach (NodeData data in nodeDataList)
            {
                Vector3 position = new Vector3(data.positionX, data.positionY, data.positionZ);
                nodeManager.AddNode(position);
                GameObject node = nodeManager.Nodes[nodeManager.Nodes.Count - 1];
                NodeData nodeData = node.GetComponent<NodeData>();
                nodeData.nodeId = data.nodeId;
                nodeData.nodeName = data.nodeName;
                nodeData.creationDate = data.creationDate;
                nodeData.updateDate = data.updateDate;
                nodeData.parentNodeId = data.parentNodeId;
                nodeData.positionX = position.x;
                nodeData.positionY = position.y;
                nodeData.positionZ = position.z;
            }
        }
    }
}
