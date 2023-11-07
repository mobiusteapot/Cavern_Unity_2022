using UnityEngine;
namespace ETC.CaveCavern
{
    [CreateAssetMenu(fileName = "CavernOutputSettings", menuName = "Cavern/CavernOutputSettings")]
    public class CavernOutputSettings : ScriptableObject
    {
        [Header("Display Render Mode")]
        public CameraOutputMode camOutputMode = CameraOutputMode.SingleDisplay;
        public Rect cropRect;

        public void Reset()
        {
            cropRect.height = 1;
            cropRect.width = 1;
        }
    }
}
