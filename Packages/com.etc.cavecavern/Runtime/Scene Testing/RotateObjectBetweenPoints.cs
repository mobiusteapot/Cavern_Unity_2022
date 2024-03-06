using System.Collections;
using UnityEngine;

public class RotateObjectBetweenPoints : MonoBehaviour
{
    public GameObject ObjectToRotate;
    public GameObject ObjectToRotateAround;

    public float RotateDuration = 5f;
    public float YRotateMax = 0f;
    public float YRotateMin = 270f;
    public float defaultOffset = 90f;
    
    private bool curRotForward = true;
    private Coroutine rotateCoroutine;
    private void Update()
    {
        if(ObjectToRotate == null || ObjectToRotateAround == null) return;
        if(rotateCoroutine == null)
        {
            rotateCoroutine = StartCoroutine(RotateObject());
        }
    }
    public void StopRotation()
    {
        if(rotateCoroutine != null)
        {
            StopCoroutine(rotateCoroutine);
            rotateCoroutine = null;
        }
    }

    IEnumerator RotateObject()
    {
        
        // Rotates ObjectToRotateAround from YRotateMin to YRotateMax
        float time = 0;
        float start = curRotForward ? YRotateMax : YRotateMin;
        float end = curRotForward ? YRotateMin : YRotateMax;
        start += defaultOffset;
        end += defaultOffset;

        ObjectToRotateAround.transform.localRotation = Quaternion.Euler(0, start, 0);
        while (time < RotateDuration)
        {
            float newY = Mathf.Lerp(start, end, time / RotateDuration);
            ObjectToRotateAround.transform.localRotation = Quaternion.Euler(0, newY, 0);
            time += Time.deltaTime;
            yield return null;
        }
        curRotForward = !curRotForward;
        rotateCoroutine = null;
    }
    // Draws a line from red (start) to green (end)
    private void OnDrawGizmos()
    {
        if(ObjectToRotate == null || ObjectToRotateAround == null) return;
        var aroundPos = ObjectToRotateAround.transform.position;
        var toPos = ObjectToRotate.transform.position;
        float distanceBetween = Vector3.Distance(aroundPos, toPos);

        Vector3 minRotatedVector = Quaternion.Euler(0, YRotateMin, 0) * ObjectToRotateAround.transform.forward;
        Vector3 maxRotatedVector = Quaternion.Euler(0, YRotateMax, 0) * ObjectToRotateAround.transform.forward;

        Gizmos.color = Color.red;
        Gizmos.DrawLine(aroundPos, aroundPos + minRotatedVector * distanceBetween);
        Gizmos.color = Color.green;
        Gizmos.DrawLine(aroundPos, aroundPos + maxRotatedVector * distanceBetween);
    }
}
