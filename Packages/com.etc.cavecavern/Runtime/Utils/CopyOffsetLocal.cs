using UnityEngine;

public class CopyOffsetLocal : MonoBehaviour
{
    [SerializeField] private Transform parent;
    public Vector3 initialVal;
    // Copies the transform of the parent object as an offset to self
    void Start()
    {
        initialVal = transform.localPosition;
    }
    void Update()
    {
        // Apply offset from start to current position
        transform.localPosition = parent.localPosition + initialVal;
    }
}
