using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Attaches a gameobject to a parent but only in terms of position, not rotation
/// </summary>
public class CopyTransformPosition : MonoBehaviour
{
    [Tooltip("Which object are we constrained to?")]
    public Transform parent;

    // What is our position in the parent's coordinate system?
    private Vector3 offset;

    public void BindToParent(Transform parent)
    {
        this.parent = parent;
        offset = parent.InverseTransformPoint(transform.position);
    }

    void Awake()
    {
        if (parent)
            BindToParent(parent);
    }

    private void LateUpdate()
    {
        if (parent)
            transform.position = parent.TransformPoint(offset);
    }
}
