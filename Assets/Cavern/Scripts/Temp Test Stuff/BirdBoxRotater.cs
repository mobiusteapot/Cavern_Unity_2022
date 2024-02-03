using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BirdBoxRotater : MonoBehaviour
{
    public float RotateSpeed = 1f;
    public float YRotateMin = 0f;
    public float YRotateMax = 270f;
    public GameObject ObjectToRotate;
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
        Vector3 distanceVector = Vector3.Normalize(transform.position - ObjectToRotate.transform.position);
        float distanceBetween = Vector3.Distance(transform.position, ObjectToRotate.transform.position);
        // Rotate the distance vector by the min and max angles
        Vector3 minRotatedVector = Quaternion.Euler(0, YRotateMin, 0) * distanceVector;
        Vector3 maxRotatedVector = Quaternion.Euler(0, YRotateMax, 0) * distanceVector;
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + minRotatedVector * -distanceBetween);
        Gizmos.color = Color.green;
        Gizmos.DrawLine(transform.position, transform.position + maxRotatedVector * -distanceBetween);
    }
}
