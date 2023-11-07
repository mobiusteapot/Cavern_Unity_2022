using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnforceRotation : MonoBehaviour
{
    [Tooltip("Which object's rotation do we copy?")]
    public Transform example;

    [Tooltip("Do we copy the x axis?")]
    public bool copyX;

    [Tooltip("Do we copy the y axis?")]
    public bool copyY;

    [Tooltip("Do we copy the z axis?")]
    public bool copyZ;

    // Update is called once per frame
    void Update()
    {
        if (!example)
            transform.rotation = Quaternion.identity;
        else
        {
            transform.localRotation = Quaternion.Euler(
                copyX ? example.rotation.eulerAngles.x : 0,
                copyY ? example.rotation.eulerAngles.y : 0,
                copyZ ? example.rotation.eulerAngles.z : 0);
        }
    }
}
