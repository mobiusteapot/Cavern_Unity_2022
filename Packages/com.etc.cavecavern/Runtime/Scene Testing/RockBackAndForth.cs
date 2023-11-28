using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockBackAndForth : MonoBehaviour
{

    public float dist = 1.5f;

    public float timeOffset;


    private Vector3 orig;
    // Start is called before the first frame update
    void Start()
    {
        orig = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = orig + transform.forward * Mathf.Sin(Time.time + timeOffset * 2 * Mathf.PI) * dist;
    }
}
