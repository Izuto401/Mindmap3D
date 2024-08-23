using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeOutline : MonoBehaviour
{
    public GameObject outlineObject;

    void Start()
    {
        outlineObject.SetActive(false);
    }

    void OnMouseEnter()
    {
        outlineObject.SetActive(true);
    }

    void OnMouseExit()
    {
        outlineObject.SetActive(false);
    }

    void OnMouseDown()
    {
        outlineObject.SetActive(true);
    }

    public void DeactivateOutline()
    {
        outlineObject.SetActive(false);
    }
}
