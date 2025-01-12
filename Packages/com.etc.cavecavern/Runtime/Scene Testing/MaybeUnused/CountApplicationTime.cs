using UnityEngine;
using UnityEngine.UI;

namespace ETC.CaveCavern
{
    public class CountApplicationTime : MonoBehaviour
    {
        private Text text;
        void Start()
        {
            text = GetComponent<Text>();
        }
        void Update()
        {
            text.text = Time.realtimeSinceStartup + "";
        }
    }
}