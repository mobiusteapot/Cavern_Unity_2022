using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace ETC.CaveCavern
{
    [CreateAssetMenu(fileName = "CavernOutputSettings", menuName = "Cavern/CavernOutputSettings")]
    public class CavernOutputSettings : ScriptableObject
    {
        public Rect cropRect;

        public void Reset()
        {
            cropRect.height = 1;
            cropRect.width = 1;
        }
    }
}
