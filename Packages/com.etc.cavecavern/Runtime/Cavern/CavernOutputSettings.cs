using UnityEngine;
namespace ETC.CaveCavern
{
    [CreateAssetMenu(fileName = "CavernOutputSettings", menuName = "Cavern/CavernOutputSettings")]
    public class CavernOutputSettings : ScriptableObject
    {
        [Header("Rig Type")]
        public CavernRigType rigType = CavernRigType.SingleCamera;
        [Header("Display Render Mode")]
        public CameraOutputMode camOutputMode = CameraOutputMode.SingleDisplay;
        public Rect cropRect;
        public Rect stretchRect;

        public void Reset()
        {
            cropRect.height = 1;
            cropRect.width = 1;
        }
    }
}
