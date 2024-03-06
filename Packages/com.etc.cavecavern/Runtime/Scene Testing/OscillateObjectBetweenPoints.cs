using System.Collections;
using UnityEngine;

/// <summary>
/// Moves this object back and forth between the two given points over time.
/// </summary>
public class OscillateObjectBetweenPoints : MonoBehaviour {
    public bool ShouldMove = true;
    public GameObject ObjectToMove;
    public float MoveDuration = 10f;
    public Transform PointA;
    public Transform PointB;
    private bool isMovingForward = true;
    public Coroutine MoveCoroutine;
    private void Update() {
        if (!ShouldMove) return;
        if (ObjectToMove == null) return;

        if (MoveCoroutine == null) {
            MoveCoroutine = StartCoroutine(MoveObject());
        }
    }
    private IEnumerator MoveObject() {
        float elapsedTime = 0;
        Vector3 startingPos = ObjectToMove.transform.position;
        Vector3 targetPos = isMovingForward ? PointB.position : PointA.position;
        while (elapsedTime < MoveDuration) {
            ObjectToMove.transform.position = Vector3.Lerp(startingPos, targetPos, (elapsedTime / MoveDuration));
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        isMovingForward = !isMovingForward;
        MoveCoroutine = null;
    }
}
