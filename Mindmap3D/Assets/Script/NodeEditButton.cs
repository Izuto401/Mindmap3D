using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeEditButton : MonoBehaviour
{
    public GameObject nodePrefab;
    public Transform parentTransform;

    public void AddNode(Vector3 position)
    {
        Instantiate(nodePrefab, position, Quaternion.identity, parentTransform);
    }

    public void DeleteNode(GameObject node)
    {
        Destroy(node);
    }
}
