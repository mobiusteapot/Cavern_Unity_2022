using UnityEngine;

namespace ETC.CaveCavern
{
    public class RockBackAndForth : MonoBehaviour
    {

        public float dist = 1.5f;
        public float timeOffset;
        private Vector3 orig;
        void Start()
        {
            orig = transform.position;
        }
        void Update()
        {
            transform.position = orig + transform.forward * Mathf.Sin(Time.time + timeOffset * 2 * Mathf.PI) * dist;
        }
    }
}