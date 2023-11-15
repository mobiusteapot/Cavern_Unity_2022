using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BirdBoxRotater : MonoBehaviour
{
    public float RotateSpeed = 1f;
    public float YRotateMin = 0f;
    public float YRotateMax = 270f;
    bool curRotForward = false;
    bool hasReachedEnd = false;


    // Update is called once per frame
    void FixedUpdate()
    {
        if (hasReachedEnd)
        {
            curRotForward = false;
            if(transform.localEulerAngles.y < YRotateMin)
            {
                hasReachedEnd = false;
                curRotForward = true;
            }
        } else
        {
            if(transform.localEulerAngles.y > YRotateMax)
            {
                hasReachedEnd = true;
            }
        }
        float currentRotationOffset = curRotForward ? RotateSpeed : -RotateSpeed;
        transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, transform.localEulerAngles.y + currentRotationOffset, transform.localEulerAngles.z);
    }

    private void OnDrawGizmos()
    {
        Vector3 minPos = transform.position;
        minPos.x += 2f;
    }
}
